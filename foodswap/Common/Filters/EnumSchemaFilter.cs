using foodswap.Features.FoodFeatures;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace foodswap.Common.Filters;
public class EnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type.IsEnum)
        {
            schema.Enum.Clear();
            foreach (var enumName in Enum.GetNames(context.Type))
            {
                var enumValue = (int)Enum.Parse(context.Type, enumName);
                if (enumValue != 0)
                {
                    schema.Enum.Add(new OpenApiString(enumName));
                }
            }
            // Enum.GetNames(context.Type)
            //     .Where(name => name != "INVALID_CATEGORY")
            //     .ToList()
            //     .ForEach(name => schema.Enum.Add(new OpenApiString(name)));
        }
    }
}