﻿
/*
  DDSchema.cs -- stored schema for serialyzing of the 'DrDataSe' general purpose Data abstraction layer 1.1, November 26, 2016
 
  Copyright (c) 2013-2016 Kudryashov Andrey aka Dr
 
  This software is provided 'as-is', without any express or implied
  warranty. In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

      1. The origin of this software must not be misrepresented; you must not
      claim that you wrote the original software. If you use this software
      in a product, an acknowledgment in the product documentation would be
      appreciated but is not required.

      2. Altered source versions must be plainly marked as such, and must not be
      misrepresented as being the original software.

      3. This notice may not be removed or altered from any source distribution.

      Kudryashov Andrey <kudryashov.andrey at gmail.com>

 */

using System;

namespace DrOpen.DrCommon.DrDataSe
{
    public static class DDSchema
    {
        #region Serialize xml
        public const string XML_SERIALIZE_NODE_VALUE = "v";
        public const string XML_SERIALIZE_NODE = "n";
        public const string XML_SERIALIZE_NODE_ATTRIBUTE_COLLECTION = "ac";
        public const string XML_SERIALIZE_NODE_ATTRIBUTE = "a";
        public const string XML_SERIALIZE_NODE_ARRAY_VALUE_ITEM = "i";
        public const string XML_SERIALIZE_ATTRIBUTE_NAME = "n";
        public const string XML_SERIALIZE_ATTRIBUTE_TYPE = "t";
        public const string XML_SERIALIZE_ATTRIBUTE_ROOT = "r";
        public const string XML_SERIALIZE_ATTRIBUTE_SIZE = "s";
        public const string XML_SERIALIZE_ATTRIBUTE_COUNT = "c";
        
        #endregion Serialize xml
        #region Serialize bin
        public const string SERIALIZE_ATTRIBUTE_NAME = "n";
        public const string SERIALIZE_NODE_ATTRIBUTE_COLLECTION = "ac";
        public const string SERIALIZE_NODE_ARRAY_VALUE_ITEM = "i";
        public const string SERIALIZE_NODE_ATTRIBUTE = "a";
        public const string SERIALIZE_ATTRIBUTE_TYPE = "t";
        public const string SERIALIZE_ATTRIBUTE_COUNT = "c";

        #endregion Serialize bin

        #region string format
        public const string StringDateTimeFormat = "o"; //ISO 8601 format
        public const string StringRoundTripFormat = "r"; //round-trip format for Single, Double, and BigInteger types.
        #endregion string format
    }
}