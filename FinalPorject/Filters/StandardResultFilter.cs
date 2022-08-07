using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Utils;

namespace Filters
{
    public class StandardResultFilter : ActionFilterAttribute
    {

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is BadRequestObjectResult badRequestObjectResult)
            {
                var errs = context.ModelState.Values;
                var list = (from err in errs from innerError in err.Errors select innerError.ErrorMessage).ToList();
                var message = string.Join(" | ", list);
                throw new AppException(message, 400);
            }

            base.OnResultExecuting(context);
        }
    }
}
