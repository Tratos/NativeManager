﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using NativeManager.WinApi;

namespace NativeManager.ProcessInteraction
{
    public unsafe class ProcessInfo
    {
        private readonly Process m_Process;

        public ProcessInfo(Process process) => m_Process = process;

        public ProcessModule GetModule(string module)
        {
            if (m_Process.Modules.Count == 0)
            {
                throw new InvalidOperationException("Modules equals zero.");
            }

            var processModule = m_Process.Modules.Cast<ProcessModule>().FirstOrDefault(mdl => mdl.ModuleName == module);

            if (processModule == null)
            {
                throw new DllNotFoundException($"Could not find library at given address.");
            }

            return processModule;
        }

        public ProcessModule GetModule(IntPtr modulePtr)
        {
            if (m_Process.Modules.Count == 0)
            {
                throw new InvalidOperationException("Modules equals zero.");
            }

            var processModule = m_Process.Modules.Cast<ProcessModule>().FirstOrDefault(mdl => mdl.BaseAddress == modulePtr);

            if (processModule == null)
            {
                throw new DllNotFoundException($"Could not find library at given address.");
            }

            return processModule;
        }

        public Dictionary<string, ProcessModule> GetModules()
        {
            Dictionary<string, ProcessModule> Modules = new Dictionary<string, ProcessModule>();

            if (m_Process.Modules.Count == 0)
            {
                throw new InvalidOperationException("Modules equals zero");
            }

            m_Process.Modules.Cast<ProcessModule>().All(mdl =>
            {
                Modules.Add(mdl.ModuleName, mdl);

                return true;
            });

            return Modules;
        }

        public Dictionary<string, IntPtr> GetModulesAddress()
        {
            Dictionary<string, IntPtr> Modules = new Dictionary<string, IntPtr>();

            foreach(var module in GetModules())
            {
                Modules.Add(module.Key, module.Value.BaseAddress);
            }

            return Modules;
        }

        public bool IsActiveWindow() => m_Process.MainWindowHandle == User32.GetForegroundWindow();

        public static ProcessModule GetModule(Process process,string module)
        {
            if (process.Modules.Count == 0)
            {
                throw new InvalidOperationException("Modules equals zero.");
            }

            var processModule = process.Modules.Cast<ProcessModule>().FirstOrDefault(mdl => mdl.ModuleName == module);

            if (processModule == null)
            {
                throw new DllNotFoundException($"Could not find library at given address.");
            }

            return processModule;
        }

        public static ProcessModule GetModule(Process process, IntPtr modulePtr)
        {
            if (process.Modules.Count == 0)
            {
                throw new InvalidOperationException("Modules equals zero.");
            }

            var processModule = process.Modules.Cast<ProcessModule>().FirstOrDefault(mdl => mdl.BaseAddress == modulePtr);

            if (processModule == null)
            {
                throw new DllNotFoundException($"Could not find library at given address.");
            }

            return processModule;
        }

        public static Dictionary<string, ProcessModule> GetModules(Process process)
        {
            Dictionary<string, ProcessModule> Modules = new Dictionary<string, ProcessModule>();

            if (process.Modules.Count == 0)
            {
                throw new InvalidOperationException("Modules equals zero");
            }

            process.Modules.Cast<ProcessModule>().All(mdl =>
            {
                Modules.Add(mdl.ModuleName, mdl);

                return true;
            });

            return Modules;
        }

        public static Dictionary<string, IntPtr> GetModulesAddress(Process process)
        {
            Dictionary<string, IntPtr> Modules = new Dictionary<string, IntPtr>();

            foreach (var module in GetModules(process))
            {
                Modules.Add(module.Key, module.Value.BaseAddress);
            }

            return Modules;
        }

        internal static void Exists(Process[] process, int index = 0)
        {
            if (process.Length != 0 && index <= (process.Length - 1))
            {
                return;
            }

            throw new InvalidOperationException("Process not found");
        }
    }
}