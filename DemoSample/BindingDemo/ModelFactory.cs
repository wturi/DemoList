using System;
using System.Collections.Generic;
using System.Windows;

namespace BindingDemo
{
    public static class ModelFactory
    {
        public static object Get(Type mType, object[] objects)
        {
            var model = Application.Current.Properties[nameof(ModelFactory)] as Dictionary<string, object>;
            if (model.ContainsKey(mType.FullName))
                return model[mType.FullName];
            else
            {
                var newModel = Activator.CreateInstance(mType, objects);
                model.Add(mType.FullName, newModel);
                return newModel;
            }
        }
    }
}