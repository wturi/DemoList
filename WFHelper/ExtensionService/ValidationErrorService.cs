using System;
using System.Activities.Presentation.Validation;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFHelper.ExtensionService
{
    /// <summary>
    /// 验证错误的服务
    /// </summary>
 public   class ValidationErrorService : IValidationErrorService
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
