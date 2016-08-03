﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using VIC.DataAccess.Abstraction;
using VIC.DataAccess.Abstraction.Converter;
using VIC.DataAccess.Core;
using VIC.DataAccess.Core.Converter;
using VIC.DataAccess.MSSql.Core;

namespace VIC.DataAccess
{
    public static partial class DataAccessExtensions
    {
        public static IServiceCollection UseDataAccess(this IServiceCollection service)
        {
            TypeHelper.SqlParameterType = typeof(SqlParameter);
            return service.AddSingleton<IDbFuncNameConverter, DbFuncNameConverter>()
                .AddSingleton<IDbTypeConverter, DbTypeConverter>()
                .AddSingleton<IScalarConverter, ScalarConverter>()
                .AddSingleton<IEntityConverter, EntityConverter>()
                .AddSingleton<IParamConverter, ParamConverter>()
                .AddTransient<IDataCommand, MSSqlDataCommand>();
        }

        public static Task ExecuteBulkCopyAsync<T>(this IDataCommand command, List<T> data) where T : class, new()
        {
            var msCommand = command as MSSqlDataCommand;
            if (msCommand == null) throw new ArgumentException("command is not MSSqlDataCommand");
            return msCommand.ExecuteBulkCopyAsync(data);
        }
    }
}