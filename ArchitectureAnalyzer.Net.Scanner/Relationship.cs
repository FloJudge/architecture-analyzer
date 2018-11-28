﻿
namespace ArchitectureAnalyzer.Net.Scanner
{
    public static class Relationship
    {
        public const string DEFINES_TYPE = "DEFINES_TYPE";

        public const string DEFINES_METHOD = "DEFINES_METHOD";

        public const string DEFINES_PROPERTY = "DEFINES_PROPERTY";

        public const string DEFINES_PARAMETER = "DEFINES_PARAMETER";

        public const string DEFINES_FIELD = "DEFINES_FIELD";

        public const string DEFINES_GENERIC_TYPE_ARG = "DEFINES_GENERIC_TYPE_ARG";

        public const string DEFINES_GENERIC_METHOD_ARG = "DEFINES_GENERIC_METHOD_ARG";

        public const string EXTENDS = "EXTENDS";

        public const string IMPLEMENTS = "IMPLEMENTS";

        public const string EXPORTS = "EXPORTS";

        public const string IMPORTS = "IMPORTS";

        public const string DEPENDS_ON = "DEPENDS_ON";

        public const string RETURNS = "RETURNS";
        
        public const string HAS_TYPE = "HAS_TYPE";

        public const string HAS_TYPE_ARGUMENT = "HAS_TYPE_ARGUMENT";
        
        public const string HAS_ATTRIBUTE = "HAS_ATTRIBUTE";

        public const string INSTANTIATES_GENERIC_TYPE = "INSTANTIATES_GENERIC_TYPE";
    }
}
