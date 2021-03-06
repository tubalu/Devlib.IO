﻿//-----------------------------------------------------------------------
// <copyright file="DaemonProcessHelper.cs" company="YuGuan Corporation">
//     Copyright (c) YuGuan Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace DevLib.DaemonProcess
{
    using System;
    using System.Collections.Generic;
    using System.Management;
    using System.Security.Permissions;
    using System.Text;
    using DevLib.DaemonProcess.NativeAPI;

    /// <summary>
    /// Class DaemonProcessHelper.
    /// </summary>
    public static class DaemonProcessHelper
    {
        /// <summary>
        /// Get command line string by process Id.
        /// </summary>
        /// <param name="processId">Process id.</param>
        /// <returns>Command line string.</returns>
        [EnvironmentPermissionAttribute(SecurityAction.LinkDemand, Unrestricted = true)]
        public static string GetCommandLineByProcessId(int processId)
        {
            string result = string.Empty;

            ManagementObjectSearcher managementObjectSearcher = null;

            try
            {
                managementObjectSearcher = new ManagementObjectSearcher(string.Format("SELECT CommandLine FROM Win32_Process WHERE ProcessId = {0}", processId.ToString()));

                foreach (ManagementObject managementObject in managementObjectSearcher.Get())
                {
                    try
                    {
                        result = managementObject["CommandLine"].ToString();

                        break;
                    }
                    catch (Exception e)
                    {
                        InternalLogger.Log(e);
                    }
                }
            }
            catch
            {
                result = NativeMethodsHelper.GetCommandLine(processId);
            }
            finally
            {
                if (managementObjectSearcher != null)
                {
                    managementObjectSearcher.Dispose();
                    managementObjectSearcher = null;
                }
            }

            return result;
        }

        /// <summary>
        /// Get argument list.
        /// </summary>
        /// <param name="args">Arguments string.</param>
        /// <returns>Argument list.</returns>
        public static List<string> GetArguments(string args)
        {
            List<string> result = new List<string>();

            if (string.IsNullOrEmpty(args))
            {
                return result;
            }

            StringBuilder stringBuilder = new StringBuilder();

            int splitSwitch = 0;

            foreach (char item in args.Trim())
            {
                if (splitSwitch % 2 == 1)
                {
                    if (!item.Equals('\"'))
                    {
                        stringBuilder.Append(item);
                        continue;
                    }
                    else
                    {
                        splitSwitch += 1;
                        result.Add(stringBuilder.ToString());
                        stringBuilder.Length = 0;
                        continue;
                    }
                }
                else
                {
                    if (item.Equals('\"'))
                    {
                        splitSwitch += 1;
                        continue;
                    }

                    if (!item.Equals(' '))
                    {
                        stringBuilder.Append(item);
                        continue;
                    }
                    else
                    {
                        if (stringBuilder.Length > 0)
                        {
                            result.Add(stringBuilder.ToString());
                            stringBuilder.Length = 0;
                            continue;
                        }
                    }
                }
            }

            if (stringBuilder.Length > 0)
            {
                result.Add(stringBuilder.ToString());
            }

            return result;
        }

        /// <summary>
        /// Get argument list from command line.
        /// </summary>
        /// <param name="commandLine">Command line string.</param>
        /// <returns>Command line argument list.</returns>
        public static List<string> GetCommandLineArguments(string commandLine)
        {
            List<string> result = new List<string>();

            if (string.IsNullOrEmpty(commandLine))
            {
                return result;
            }

            string args = null;

            commandLine = commandLine.Trim();

            if (commandLine.StartsWith("\""))
            {
                int argsIndex = commandLine.IndexOf("\"", 1);
                args = commandLine.Substring(argsIndex + 1, commandLine.Length - argsIndex - 1);
            }
            else
            {
                int argsIndex = commandLine.IndexOf(" ", 0);
                args = commandLine.Substring(argsIndex + 1, commandLine.Length - argsIndex - 1);
            }

            if (string.IsNullOrEmpty(args))
            {
                return result;
            }

            StringBuilder stringBuilder = new StringBuilder();

            int splitSwitch = 0;

            foreach (char item in args.Trim())
            {
                if (splitSwitch % 2 == 1)
                {
                    if (!item.Equals('\"'))
                    {
                        stringBuilder.Append(item);
                        continue;
                    }
                    else
                    {
                        splitSwitch += 1;
                        result.Add(stringBuilder.ToString());
                        stringBuilder.Length = 0;
                        continue;
                    }
                }
                else
                {
                    if (item.Equals('\"'))
                    {
                        splitSwitch += 1;
                        continue;
                    }

                    if (!item.Equals(' '))
                    {
                        stringBuilder.Append(item);
                        continue;
                    }
                    else
                    {
                        if (stringBuilder.Length > 0)
                        {
                            result.Add(stringBuilder.ToString());
                            stringBuilder.Length = 0;
                            continue;
                        }
                    }
                }
            }

            if (stringBuilder.Length > 0)
            {
                result.Add(stringBuilder.ToString());
            }

            return result;
        }

        /// <summary>
        /// Compare if two argument lists are equal or not.
        /// </summary>
        /// <param name="leftArgs">The first argument list.</param>
        /// <param name="rightArgs">The second argument list.</param>
        /// <param name="comparisonType">One of the enumeration values that specifies the rules to use in the comparison.</param>
        /// <returns>true if two command line arguments are equal;otherwise, false.</returns>
        public static bool CommandLineArgumentsEquals(IList<string> leftArgs, IList<string> rightArgs, StringComparison comparisonType)
        {
            if (object.ReferenceEquals(leftArgs, rightArgs))
            {
                return true;
            }

            if (object.ReferenceEquals(leftArgs, null) || object.ReferenceEquals(rightArgs, null))
            {
                return false;
            }

            if (leftArgs.Count != rightArgs.Count)
            {
                return false;
            }

            for (int i = 0; i < leftArgs.Count; i++)
            {
                if (!leftArgs[i].Trim('\"').Equals(rightArgs[i].Trim('\"'), comparisonType))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
