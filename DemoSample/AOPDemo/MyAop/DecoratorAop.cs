using System;

using Newtonsoft.Json;

namespace MyAop
{
    /// <summary>
    /// 装饰器模式实现静态代理
    /// AOP 在方法前后增加自定义的方法
    ///
    /// 简单的proxy静态AOP
    /// </summary>
    public class DecoratorAop
    {
        public static void Show()
        {
            var user = new User()
            {
                Name = "老王",
                Password = "123123123123"
            };
            IUserProcessor processor = new UserProcessorDecorator(new UserProcessor());
            processor.RegUser(user);//使用了AOP
        }

        private interface IUserProcessor
        {
            void RegUser(User user);
        }

        private class UserProcessor : IUserProcessor
        {
            public void RegUser(User user)
            {
                Console.WriteLine("用户已注册。Name:{0},PassWord:{1}", user.Name, user.Password);
            }
        }

        /// <summary>
        /// 装饰器的模式去提供一个AOP功能
        /// </summary>
        private class UserProcessorDecorator : IUserProcessor
        {
            private IUserProcessor UserProcessor { get; set; }

            public UserProcessorDecorator(IUserProcessor userProcessor)
            {
                UserProcessor = userProcessor;
            }

            public void RegUser(User user)
            {
                BeforeProceed(user);

                this.UserProcessor.RegUser(user);

                AfterProceed(user);
            }

            /// <summary>
            /// 业务逻辑之前
            /// </summary>
            /// <param name="user"></param>
            private void BeforeProceed(User user)
            {
                Console.WriteLine($"方法执行前,{JsonConvert.SerializeObject(user)}");
            }

            /// <summary>
            /// 业务逻辑之后
            /// </summary>
            /// <param name="user"></param>
            private void AfterProceed(User user)
            {
                Console.WriteLine($"方法执行后,{JsonConvert.SerializeObject(user)}");
            }
        }

        private class User
        {
            public string Name { get; set; }
            public string Password { get; set; }
        }
    }
}