// =========================================================================
// Copyright 2020 EPAM Systems, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// =========================================================================

using System;
using Xamarin.Forms;

namespace Covi.Features.ImagesScaling
{
    public class ImageScaleConverter : IValueConverter
    {
        private const string AddressFormat = "?width={0}&height={1}";

        public double ImageWidth { get; set; }

        public double ImageHeight { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var result = value;
            if (value is string imageAddress)
            {
                var scale = Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Density;
                var formattedParameters = string.Format(AddressFormat, (int)(ImageWidth * scale), (int)(ImageHeight * scale));
                result = imageAddress + formattedParameters;
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
