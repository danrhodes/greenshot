/*
 * Greenshot - a free and open source screenshot tool
 * Copyright (C) 2007-2021 Thomas Braun, Jens Klingen, Robin Krom
 *
 * For more information see: https://getgreenshot.org/
 * The Greenshot project is hosted on GitHub https://github.com/greenshot/greenshot
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 1 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using System;
using System.Windows.Automation;
using log4net;

namespace Greenshot.Base.Interop
{
    /// <summary>
    /// Helper class for retrieving window information via UI Automation.
    /// Used as fallback for Chromium-based browsers where GetWindowText fails.
    /// </summary>
    public static class UIAutomationHelper
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(UIAutomationHelper));

        /// <summary>
        /// Gets the window title using UI Automation API.
        /// This works across security boundaries where GetWindowText may fail.
        /// </summary>
        /// <param name="hWnd">The window handle</param>
        /// <returns>The window title, or null if retrieval fails</returns>
        public static string GetWindowTitle(IntPtr hWnd)
        {
            if (hWnd == IntPtr.Zero)
            {
                return null;
            }

            try
            {
                var element = AutomationElement.FromHandle(hWnd);
                return element?.Current.Name;
            }
            catch (ElementNotAvailableException)
            {
                // Window was closed or became unavailable - this is expected
                return null;
            }
            catch (Exception ex)
            {
                Log.Debug("Failed to get window title via UI Automation", ex);
                return null;
            }
        }
    }
}
