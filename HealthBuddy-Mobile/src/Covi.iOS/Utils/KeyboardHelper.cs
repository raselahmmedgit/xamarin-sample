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
using CoreGraphics;
using Covi.Features.Main;
using Foundation;
using UIKit;
using Xamarin.Forms;

namespace Covi.iOS.Utils
{
    public static class KeyboardHelper
    {
        private static NSObject _keyboardHideObserver;
        private static NSObject _keyboardShowObserver;

        private static nfloat _appliedPadding;

        public static void Initialize()
        {
            if (_keyboardShowObserver == null)
            {
                _keyboardShowObserver = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, OnKeyboardShow);
            }

            if (_keyboardHideObserver == null)
            {
                _keyboardHideObserver = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, OnKeyboardHide);
            }
        }

        private static void OnKeyboardShow(NSNotification notification)
        {
            OnKeyboardUpdate(notification, true);
        }

        private static void OnKeyboardHide(NSNotification notification)
        {
            OnKeyboardUpdate(notification, false);
        }

        private static void OnKeyboardUpdate(NSNotification notification, bool isKeyboardShown)
        {
            var keyboardFrame = UIKeyboard.FrameEndFromNotification(notification);

            if (App.Current.MainPage is Covi.Features.Shell.ShellPage shell)
            {
                var padding = shell.CurrentPage.Padding;

                var keyboardHeight = keyboardFrame.Height;
                keyboardHeight = UpdateKeyboardHeightIfNeeded(keyboardHeight, shell.CurrentPage);
                var deltaPadding = UpdateAppliedPaddingAndGetDelta(keyboardHeight, isKeyboardShown);

                padding = new Thickness(padding.Left, padding.Top, padding.Right, padding.Bottom + deltaPadding);
                shell.CurrentPage.Padding = padding;
            }
        }

        private static nfloat UpdateKeyboardHeightIfNeeded(nfloat height, Page currentPage)
        {
            if (currentPage is MainPage tabbedPage)
            {
                height = height - tabbedPage.TabBarHeight;
            }
            else if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            {
                height = height - UIApplication.SharedApplication.KeyWindow.SafeAreaInsets.Bottom;
            }

            return height;
        }

        private static nfloat UpdateAppliedPaddingAndGetDelta(nfloat keyboardHeight, bool isKeyboardShown)
        {
            nfloat result;
            if (!isKeyboardShown)
            {
                result = -_appliedPadding;
                _appliedPadding = 0;
                return result;
            }

            result = keyboardHeight - _appliedPadding;
            _appliedPadding = keyboardHeight;
            return result;
        }
    }
}
