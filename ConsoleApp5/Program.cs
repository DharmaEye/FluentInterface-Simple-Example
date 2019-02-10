using System;
using System.Collections;

namespace ConsoleApp5
{
    public interface IFluentInterface
    {
        IFluentInterfaceChain It<T>(T it);
    }

    public interface IFluentInterfaceChain
    {
        IFluentInterfaceChain IsTypeOf<T>();
        IFluentInterfaceChain Then(Action action);
        IFluentInterfaceChain IsNotNull();
        IFluentInterfaceChain Otherwise(Action action);
        IFluentInterfaceChain And(object it);
    }

    public class FluentInterfaceChain : IFluentInterface, IFluentInterfaceChain
    {
        private readonly Stack _stack = new Stack();
        private object _it;

        public IFluentInterfaceChain It<T>(T it)
        {
            _it = it;
            return this;
        }

        IFluentInterfaceChain IFluentInterfaceChain.IsTypeOf<T>()
        {
            _stack.Push(_it is T);
            return this;
        }

        IFluentInterfaceChain IFluentInterfaceChain.Then(Action action)
        {
            var par = _stack.Pop();
            _stack.Push(par);
            if (par is bool @bool && !@bool)
            {
                return this;
            }

            action.Invoke();
            return this;
        }

        IFluentInterfaceChain IFluentInterfaceChain.IsNotNull()
        {
            _stack.Push(_it != null);
            return this;
        }

        IFluentInterfaceChain IFluentInterfaceChain.Otherwise(Action action)
        {
            var par = _stack.Pop();
            _stack.Push(par);
            if (par is bool @bool && @bool)
            {
                return this;
            }

            action.Invoke();
            return this;
        }

        IFluentInterfaceChain IFluentInterfaceChain.And(object it)
        {
            _it = it;
            return this;
        }
    }

    public class Program : FluentInterfaceChain
    {
        public static void Main()
        {
            var main = new Program();
            main.Test();

            Console.ReadLine();
        }

        public void Test()
        {
            It("test")
                .IsNotNull()
                .And("Nah")
                .IsTypeOf<object>()
                .Then(() => Console.WriteLine("Test"))
                .Then(() => Console.WriteLine("Something ..."))
                .Otherwise(() =>
                {
                    Console.WriteLine("otherwise");
                });
        }

    }
}