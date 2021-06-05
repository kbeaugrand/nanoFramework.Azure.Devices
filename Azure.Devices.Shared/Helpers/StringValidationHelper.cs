﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Azure.Devices.Client.Extensions;
using System;
using System.Text;

namespace Microsoft.Azure.Devices.Client
{
    internal class StringValidationHelper
    {
        private const char Base64Padding = '=';

        private static readonly char[] s_base64Table = new char[]
        {
            'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O',
            'P','Q','R','S','T','U','V','W','X','Y','Z','a','b','c','d',
            'e','f','g','h','i','j','k','l','m','n','o','p','q','r','s',
            't','u','v','w','x','y','z','0','1','2','3','4','5','6','7',
            '8','9','+','/'
        };

        public static void EnsureBase64String(string value, string paramName)
        {
            if (!IsBase64StringValid(value))
            {
                throw new ArgumentException(paramName);
            }
        }

        public static bool IsBase64StringValid(string value)
        {
            return value != null && IsBase64String(value);
        }

        public static void EnsureNullOrBase64String(string value, string paramName)
        {
            if (!IsNullOrBase64String(value))
            {
                throw new ArgumentException(paramName);
            }
        }

        public static bool IsNullOrBase64String(string value)
        {
            return value == null || IsBase64String(value);
        }

        public static bool IsBase64String(string value)
        {
            // Original code
            // value = value.Replace("\r", string.Empty)
            //    .Replace("\n", string.Empty);

            var cars = value.ToCharArray();
            StringBuilder str = new StringBuilder();
            foreach (char car in cars)
            {
                if ((car != '\r') || (car != '\n'))
                {
                    str.Append(car);
                }
            }

            value = str.ToString();

            if (value.Length == 0 || value.Length % 4 != 0)
            {
                return false;
            }

            int lengthNoPadding = value.Length;
            value = value.TrimEnd(Base64Padding);
            int lengthPadding = value.Length;

            if (lengthNoPadding - lengthPadding > 2)
            {
                return false;
            }

            foreach (char c in value)
            {
                if (!s_base64Table.Contains(c))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
