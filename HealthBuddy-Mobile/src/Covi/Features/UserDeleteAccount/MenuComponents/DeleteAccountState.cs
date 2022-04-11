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

using System.Collections.Generic;

namespace Covi.Features.UserDeleteAccount.MenuComponents
{
    public class DeleteAccountState
    {
        public DeleteAccountContentState State { get; private set; }

        public IList<DeleteAccountMenuItemViewModel> MenuItemsList { get; private set; }

        public DeleteAccountState()
        {
            State = DeleteAccountContentState.Default;
        }

        public static DeleteAccountState CreateDefaultState()
        {
            return new DeleteAccountState();
        }

        public static DeleteAccountState CreateMenuState(IList<DeleteAccountMenuItemViewModel> menuItemsList)
        {
            return new DeleteAccountState()
            {
                State = DeleteAccountContentState.LoggedIn,
                MenuItemsList = menuItemsList
            };
        }
    }
}
