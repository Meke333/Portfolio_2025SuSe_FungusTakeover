using System;
using UnityEngine;

namespace Helper
{
    public static class LayerAndMaskHelper
    {
        public static int ConvertNumberIntoLayer(int layerNumber)
        {
            LayerMask layer = 0;
            layer |= 0x1 << layerNumber;

            return layer.value;
        }

        public static T GetMaskFromType<T,U>(U type) where T : Enum where U : Enum
        {
            return (T)(object)(int)(0x1 << (int)(object)type);
        }

        public static bool IsTypeInMask<T, U>(T mask, U type) where T : Enum where U : Enum
        {
            T typeIntoMask = (T)(object)(int)(0x1 << (int)(object)type);

            return typeIntoMask.HasFlag(mask);
        }

        ///<summary> If either of two flags is 0 it will still return 0. Only if both flags are 1 will it be 1 </summary>
        public static T EnumFlagAND<T>(T enum1, T enum2) where T : Enum
        {
            if (enum1.GetType() != enum2.GetType()) //not the same enum type
                return default(T);

            int commonBitmask = (int)(object)enum1 & (int)(object)enum2;
            return (T)(object)(int)commonBitmask;
        }
        ///<summary> If either of two flags is 1 it will return 1 </summary>
        public static T EnumFlagOR<T>(T enum1, T enum2) where T : Enum
        {
            if (enum1.GetType() != enum2.GetType()) //not the same enum type
                return default(T);

            int commonBitmask = (int)(object)enum1 | (int)(object)enum2;
            return (T)(object)(int)commonBitmask;
        }
        ///<summary> If both flags share the same value (0&0 or 1&1) it will return 1 otherwise it will return 0 </summary>
        public static T EnumFlagXOR<T>(T enum1, T enum2) where T : Enum
        {
            if (enum1.GetType() != enum2.GetType()) //not the same enum type
                return default(T);

            int commonBitmask = (int)(object)enum1 ^ (int)(object)enum2;
            return (T)(object)(int)commonBitmask;
        }
        ///<summary> Reverse each bit of the entire enum, turning each 0 into 1 and each 1 into 0 </summary>
        public static T EnumFlagNegate<T>(T enum1) where T : Enum
        {
            int commonBitmask = ~(int)(object)enum1;
            return (T)(object)(int)commonBitmask;
        }
    }

}
