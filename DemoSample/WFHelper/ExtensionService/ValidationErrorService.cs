using System.Activities.Presentation.Validation;
using System.Collections;
using System.Collections.Generic;

namespace WFHelper.ExtensionService
{
    /// <summary>
    /// 验证错误的服务
    /// </summary>
    public class ValidationErrorService : IValidationErrorService
    {
        private readonly IList _errorList;

        public ValidationErrorService(IList errorList)
        {
            _errorList = errorList;
        }

        public void ShowValidationErrors(IList<ValidationErrorInfo> errors)
        {
            _errorList.Clear();
            foreach (var error in errors)
            {
                _errorList.Add(error.Message);
            }
        }
    }
}