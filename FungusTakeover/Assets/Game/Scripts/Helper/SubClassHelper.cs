
using Game.Entity;
using System;
using UnityEngine;
using Zenject;

namespace Helper
{
    /// <summary>
    /// Helper Class for SubClasses with a overarching HandlerClass
    /// </summary>
    public class SubClassHelper
    {
        [Inject] private IInstantiator _instantiator;

        /// <summary>
        /// This instantiate the SubClass (called brain) of a HandlerClass regardless of what Type it is
        /// </summary>
        /// <typeparam name="T"> The type of the SubClass (Brain)</typeparam>
        /// <param name="handlerClass"> The object instance of the handler, which usually calls this method (this) </param>
        /// <param name="subClassSuffix"> The suffix of the class to be called </param>
        /// <returns> The SubClass the Handler manages over </returns>
        /// <exception cref="InvalidOperationException"> The combination of the handlerclass name and the enum does not exist! </exception>
        public T GetNewInputBrain<T>(object handlerClass, string subClassSuffix)
        {
            T result;

            string creationType = handlerClass.GetType().ToString() + "_";

            creationType += subClassSuffix;
            //Debug.Log(creationType);
            //Debug.Log(Type.GetType(creationType).ToString());

            result = (T)_instantiator.Instantiate(Type.GetType(creationType));

            if (result == null)
                throw new InvalidOperationException(creationType + " does not exits as a " + this.GetType().ToString() + " class!");

            return result;
        }

        public T GetNewInputBrain<T>(string className)
        {
            T result;
            //Debug.Log(className);
            result = (T) _instantiator.Instantiate(Type.GetType(className));
            if (result == null)
                throw new InvalidOperationException(className + " does not exits as a " + this.GetType().ToString() + " class!");

            return result;
        }

        public T GetNewInputBrain<T>()
        {
            T result;

            result = (T)_instantiator.Instantiate(typeof(T));

            if (result == null)
                throw new InvalidOperationException("Type does not exist");

            return result;
        }

    }

}