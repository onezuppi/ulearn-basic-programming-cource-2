using System;
using System.Linq;
using System.Reflection;

namespace Documentation
{
    public class Specifier<T> : ISpecifier
    {
        private readonly Type type = typeof(T);

        public string GetApiDescription() =>
            type.GetCustomAttribute<ApiDescriptionAttribute>(true)?.Description;

        public string[] GetApiMethodNames() =>
            type.GetMethods()
                .Where(x => x.GetCustomAttribute<ApiMethodAttribute>() != null)
                .Select(x => x.Name)
                .ToArray();

        public string GetApiMethodDescription(string methodName) =>
            type.GetMethod(methodName)?.GetCustomAttribute<ApiDescriptionAttribute>(true)?.Description;

        public string[] GetApiMethodParamNames(string methodName) =>
            type.GetMethod(methodName)?.GetParameters()
                .Select(x => x.Name)
                .ToArray();

        public string GetApiMethodParamDescription(string methodName, string paramName) =>
            GetParameterInfo(type.GetMethod(methodName), paramName)?
                .GetCustomAttribute<ApiDescriptionAttribute>(true)?.Description;

        public ApiParamDescription GetApiMethodParamFullDescription(string methodName, string paramName) =>
            GetParamFullDescription(GetParameterInfo(type.GetMethod(methodName), paramName), paramName);


        public ApiMethodDescription GetApiMethodFullDescription(string methodName)
        {
            var method = type.GetMethod(methodName);

            return method?.GetCustomAttribute<ApiMethodAttribute>() == null
                ? null
                : new ApiMethodDescription
                {
                    ReturnDescription = method.ReturnType == typeof(void)
                        ? null
                        : GetParamFullDescription(method.ReturnParameter),
                    MethodDescription = new CommonDescription(methodName, GetApiMethodDescription(methodName)),
                    ParamDescriptions = GetApiMethodParamNames(methodName)
                        .Select(paramName => GetApiMethodParamFullDescription(methodName, paramName))
                        .ToArray()
                };
        }

        private static ParameterInfo GetParameterInfo(MethodInfo method, string paramName) =>
            method?.GetParameters().FirstOrDefault(x => x.Name == paramName);

        private static ApiParamDescription GetParamFullDescription(ParameterInfo parameterInfo,
            string parameterName = null)
        {
            var apiIntValidationAttribute = parameterInfo?.GetCustomAttribute<ApiIntValidationAttribute>();
            return new ApiParamDescription
            {
                MinValue = apiIntValidationAttribute?.MinValue,
                MaxValue = apiIntValidationAttribute?.MaxValue,
                Required = parameterInfo?.GetCustomAttribute<ApiRequiredAttribute>()?.Required ?? false,
                ParamDescription = new CommonDescription(parameterName,
                    parameterInfo?.GetCustomAttribute<ApiDescriptionAttribute>()?.Description)
            };
        }
    }
}