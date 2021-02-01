﻿namespace Craftsman.Helpers
{
    using Craftsman.Enums;
    using Craftsman.Exceptions;
    using Craftsman.Models;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Utilities
    {
        public static string PropTypeCleanup(string prop)
        {
            var lowercaseProps = new string[] { "string", "int", "decimal", "double", "float", "object", "bool", "byte", "char", "byte", "ushort", "uint", "ulong" };
            if (lowercaseProps.Contains(prop.ToLower()))
                return prop.ToLower();
            else if (prop.ToLower() == "datetime")
                return "DateTime";
            else if (prop.ToLower() == "datetime?")
                return "DateTime?";
            else if (prop.ToLower() == "datetimeoffset")
                return "DateTimeOffset";
            else if (prop.ToLower() == "datetimeoffset?")
                return "DateTimeOffset?";
            else if (prop.ToLower() == "guid")
                return "Guid";
            else
                return prop;
        }

        public static string GetRepositoryName(string entityName, bool isInterface)
        {
            return isInterface ? $"I{entityName}Repository" : $"{entityName}Repository";
        }

        public static string GetIdentitySeederName(ApplicationUser user)
        {
            return user.SeederName ?? $"{user.UserName}Seeder" ?? $"{user.FirstName.Substring(0, 1)}{user.LastName}Seeder";
        }

        public static string GetControllerName(string entityName)
        {
            return $"{entityName}Controller";
        }

        public static string GetSeederName(Entity entity)
        {
            return $"{entity.Name}Seeder";
        }

        public static string GetRepositoryListMethodName(string pluralEntity)
        {
            return $"Get{pluralEntity}Async";
        }

        public static string GetAppSettingsName(string envName, bool asJson = true)
        {
            if(envName == "Startup")
                return asJson ? $"appsettings.json" : $"appsettings";

            return asJson ? $"appsettings.{envName}.json" : $"appsettings.{envName}";
        }

        public static string GetStartupName(string envName)
        {
            return envName == "Startup" ? "Startup" : $"Startup{envName}";
        }

        public static string GetProfileName(string entityName)
        {
            return $"{entityName}Profile";
        }

        public static string GetDtoName(string entityName, Dto dto)
        {
            switch (dto)
            {
                case Dto.Manipulation:
                    return $"{entityName}ForManipulationDto";
                case Dto.Creation:
                    return $"{entityName}ForCreationDto";
                case Dto.Update:
                    return $"{entityName}ForUpdateDto";
                case Dto.Read:
                    return $"{entityName}Dto";
                case Dto.ReadParamaters:
                    return $"{entityName}ParametersDto";
                default:
                    throw new Exception($"Name generator not configured for {Enum.GetName(typeof(Dto), dto)}");
            }
        }

        public static string GetDtoTypeName(EntityProperty property, Dto dto)
        {
            switch (dto)
            {
                case Dto.Manipulation:
                    return $"{GetDtoTypeName(property, "ForManipulationDto")}";
                case Dto.Creation:
                    return $"{GetDtoTypeName(property, "ForCreationDto")}";
                case Dto.Update:
                    return $"{GetDtoTypeName(property, "ForUpdateDto")}";
                case Dto.Read:
                    return $"{GetDtoTypeName(property, "Dto")}";
                case Dto.ReadParamaters:
                    return $"{GetDtoTypeName(property, "ParametersDto")}";
                default:
                    throw new Exception($"Name generator not configured for {Enum.GetName(typeof(Dto), dto)}");
            }
        }
        public static string GetDtoTypeName(EntityProperty entityProperty, string dtoSuffix)
        {
            string baseType = PropTypeCleanup(entityProperty.Type);

            if (entityProperty.IsArrayType)
                if (entityProperty.IsNavigationProperty)
                    return entityProperty.ArrayTypeName + "<" + baseType + dtoSuffix + ">";
                else
                    return entityProperty.ArrayTypeName + "<" + baseType + ">";

            else
                if (entityProperty.IsNavigationProperty)
                    return baseType + dtoSuffix;
                else
                    return baseType;
        }

        public static string ValidatorNameGenerator(string entityName, Validator validator)
        {
            switch (validator)
            {
                case Validator.Manipulation:
                    return $"{entityName}ForManipulationDtoValidator";
                case Validator.Creation:
                    return $"{entityName}ForCreationDtoValidator";
                case Validator.Update:
                    return $"{entityName}ForUpdateDtoValidator";
                default:
                    throw new Exception($"Name generator not configured for {Enum.GetName(typeof(Validator), validator)}");
            }
        }
    }
}
