using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;

namespace FileProcessor.ModelBinders
{
    public class DateTimeModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
                throw new ArgumentNullException(nameof(bindingContext));

            var modelName = bindingContext.ModelName;
            var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);
            if (valueProviderResult == ValueProviderResult.None)
                return Task.CompletedTask;

            bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

            var dateStr = valueProviderResult.FirstValue;
            
            if (!DateTime.TryParse(dateStr, out DateTime date))
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "DateTime should be in format 'dd/MM/yyyy HH:mm:ss'");
                return Task.CompletedTask;
            }

            bindingContext.Result = ModelBindingResult.Success(date);
            return Task.CompletedTask;
        }
    }
}
