using AltV.Net;
using AltV.Net.Async;
using AltV.Net.ColoredConsole;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AltVPlus
{
    public static class AltVDev
    {
        /// <summary>
        /// A custom made position to the LegionSquare
        /// </summary>
        public static readonly Position LegionSquare = new(236.46594f, -870.7121f, 30.290894f);

        /// <summary>
        /// A extension method to set the player model using the <see cref="PedModel"/> enum
        /// </summary>
        public static async Task SetModelAsync(this IPlayer player, PedModel model)
           => await player.SetModelAsync((uint)model);
        /// <summary>
        /// A extension method to set the weather using the <see cref="WeatherType"/> enum
        /// </summary>
        public static async Task SetWeatherAsync(this IPlayer player, WeatherType type)
            => await player.SetWeatherAsync((uint)type);
        /// <summary>
        /// A extension method to give the player a specific weapon using the <see cref="WeaponModel"/> enum and optional ammo
        /// also optional to select the weapon
        /// </summary>
        public static async Task GiveWeaponAsync(this IPlayer player, WeaponModel model, int ammmo = 0, bool selectWeapon = false)
            => await player.GiveWeaponAsync((uint)model, ammmo, selectWeapon);
        /// <summary>
        /// a extension method to respawn the player to the current position 
        /// can be used if the player died
        /// </summary>
        public static async Task RespawnAsync(this IPlayer player, TimeSpan? span = null)
            => await player.SpawnAsync(player.Position, span.HasValue ? (uint)span.Value.Milliseconds : 0);

        /// <summary>
        /// A extension method to loop a player function with a  optional given <see cref="TimeSpan"/> 
        /// default is 1 second
        /// </summary>
        public static async Task LoopAsync(this IPlayer player, Action action, TimeSpan? span = null)
        {
            await Task.Factory.StartNew(async () =>
            {
                PeriodicTimer timer = new(span ?? new(0, 0, 1));

                while (await timer.WaitForNextTickAsync() && player.TryGetPlayer(out _))
                    action();
            });
        }
        /// <summary>
        /// A extension method to log a message from a task with given typeargument
        /// </summary>
        public static async Task<T> LogAsync<T>(this Task<T> task, string message)
            => await LogAsync(task, message);
        /// <summary>
        /// A extension method to log a message from a task with given typeargument
        /// </summary>
        public static async Task<T> LogAsync<T>(this Task<T> task, Action<T> data)
            => await LogAsync(task, null, data);
        /// <summary>
        /// A extension method to log a message from a task with given typeargument
        /// </summary>
        private static async Task<T> LogAsync<T>(this Task<T> task, string? message = null, Action<T>? data = null)
        {
            T t = await task;

            if (!string.IsNullOrEmpty(message))
                AltAsync.Log(message);

            data?.Invoke(t);

            return t;
        }
        /// <summary>
        /// A extension method to log a message from a task
        /// </summary>
        public static async Task LogAsync(this Task task, string message)
        {
            await task;

            if (!string.IsNullOrEmpty(message))
                AltAsync.Log(message);
        }

        /// <summary>
        /// A extension method to log a message from a type or a optional message
        /// </summary>
        public static void Log<T>(this T type, string? msg = null)
        {
            if (type != null && string.IsNullOrEmpty(msg))
                AltAsync.Log(type.ToString());
            else
                AltAsync.Log(msg);
        }

        /// <summary>
        /// A extension method to check if a player exist or is not null
        /// returns true if success
        /// </summary>
        public static bool TryGetPlayer(this IPlayer aplayer, out IPlayer player)
        {
            player = aplayer;
            return aplayer is not null && aplayer.Exists;
        }
        /// <summary>
        /// Will print out `[+] YourResource.dll++`
        /// </summary>
        public static void LogStarted()
            => Alt.LogColored(new ColoredMessage() + TextColor.White + "[" + TextColor.Yellow + "+" + TextColor.White + "] " + TextColor.Cyan + $"{Assembly.GetExecutingAssembly().GetName().Name}++");
        /// <summary>
        /// Will print out `[-] YourResource.dll--`
        /// </summary>
        public static void LogStopped()
            => Alt.LogColored(new ColoredMessage() + TextColor.White + "[" + TextColor.Yellow + "-" + TextColor.White + "] " + TextColor.Magenta + $"{Assembly.GetExecutingAssembly().GetName().Name}--");

    }
}
