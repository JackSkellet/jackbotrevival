using Discord;
using Discord.Commands;
using System.Linq;
using System.Threading.Tasks;

namespace Example.Modules
{
    [Name("Convertions")]
    public class Convertions : ModuleBase<SocketCommandContext>
    {
        [Command("Celsius"), Alias("C")]
        [Summary("Convert Celsius into either Fahrenheit or Kelvin")]
        public async Task Celcius(double input)
        {

            var fahrenheit = (input * 9 / 5) + 32;
            var kelvin = input + 273.15;

            var embed = new EmbedBuilder
            {
                Title = $"Converted {input}°C",
                Description = $"Fahrenheit: {fahrenheit}°F\nKelvin: {kelvin}°K",
                Color = new Color(0xA94114)
            };

            await ReplyAsync("", embed: embed.Build());


        }
        [Command("Fahrenheit"), Alias("F")]
        [Summary("Convert Fahrenheit to Celsius & Kelvin")]
        public async Task Fahrenheit(double input)
        {
            var celsius = (input - 32) * 5 / 9;
            var kelvin = ((input - 32) * 5 / 9) + 273.15;

            var embed = new EmbedBuilder
            {
                Title = $"Converted {input}°F",
                Description = $"Celsius: {celsius}°C\nKelvin: {kelvin}°K",
                Color = new Color(0xA94114)
            };
            await ReplyAsync("", embed: embed.Build());
        }
        [Command("Kelvin"), Alias("K")]
        [Summary("Convert Kelvin to Celsius & Fahrenheit")]
        public async Task Kelvin(double input)
        {
            var celsius = input - 273.15;
            var fahrenheit = ((input - 273.15) * 9 / 5) + 32;

            var embed = new EmbedBuilder
            {
                Title = $"Converted {input}°K",
                Description = $"Celsius: {celsius}°C\nFahrenheit: {fahrenheit}°F",
                Color = new Color(0xA94114)
            };
            await ReplyAsync("", embed: embed.Build());
        }
        [Command("kilogram"), Alias("kg")]
        [Summary("Convert KG to lbs")]
        public async Task Celdius(double input)
        {
            var lbs = (input / 0.45359237);


            var embed = new EmbedBuilder
            {
                Title = $"Converted {input}KG",
                Description = $"lbs: {lbs}",
                Color = new Color(0xA94114)
            };

            await ReplyAsync("", embed: embed.Build());
        }
        [Command("lbs")]
        [Summary("Convert lbs to kg")]
        public async Task Cefius(double input)
        {
            var lbs = (input * 0.45359237);


            var embed = new EmbedBuilder
            {
                Title = $"Converted {input}lbs",
                Description = $"KG: {lbs}",
                Color = new Color(0xA94114)
            };

            await ReplyAsync("", embed: embed.Build());
        }
        [Command("miles")]
        [Summary("Convert miles tto other")]
        public async Task Celgus(double input)
        {
            var kilometers = (input * 1.609344);
            var feet = (input / 5280);
            var inch = (input * 63360);
            var meter = (input * 1609.344);
            var cm = (input * 160934.4);

            var embed = new EmbedBuilder
            {
                Title = $"Converted {input}Miles too",
                Description = $"Kilometers: {kilometers}km\nFeet: {feet}ft\nInches: {inch}in\nMeter: {meter}M\nCentimeters: {cm}cm",
                Color = new Color(0xA94114)
            };

            await ReplyAsync("", embed: embed.Build());
        }

        [Command("km")]
        [Summary("Convert kilometers to other")]
        public async Task Csius(double input)
        {
            var kilometers = (input / 1.609344);
            var feet = (input / 0.0003048);
            var inch = (input / 0.0000254);
            var meter = (input * 1000);
            var cm = (input * 100000);

            var embed = new EmbedBuilder
            {
                Title = $"Converted {input}Kilometers too",
                Description = $"Miles: {kilometers}miles\nFeet: {feet}ft\nInches: {inch}in\nMeter: {meter}M\nCentimeters: {cm}cm",
                Color = new Color(0xA94114)
            };

            await ReplyAsync("", embed: embed.Build());
        }

        [Command("feet")]
        [Summary("Convert feet to other")]
        public async Task Cssus(double input)
        {
            var miles = (input / 5280);
            var km = (input * 0.0003048);
            var inch = (input * 12);
            var meter = (input * 0.3048);
            var cm = (input * 30.48);

            var embed = new EmbedBuilder
            {
                Title = $"Converted {input}Feet too",
                Description = $"Miles: {miles}miles\nKilometers: {km}km\nInches: {inch}in\nMeter: {meter}M\nCentimeters: {cm}cm",
                Color = new Color(0xA94114)
            };

            await ReplyAsync("", embed: embed.Build());
        }

        [Command("inch")]
        [Summary("Convert feet to other")]
        public async Task Csggsus(double input)
        {
            var miles = (input / 63360);
            var km = (input * 0.0000254);
            var feet = (input / 12);
            var meter = (input * 0.0254);
            var cm = (input * 2.54);

            var embed = new EmbedBuilder
            {
                Title = $"Converted {input}Inches too",
                Description = $"Miles: {miles}miles\nKilometers: {km}km\nFeet: {feet}ft\nMeters: {meter}M\nCentimeters: {cm}cm",
                Color = new Color(0xA94114)
            };

            await ReplyAsync("", embed: embed.Build());
        }

        [Command("meter")]
        [Summary("Convert meter to other")]
        public async Task Csggffsus(double input)
        {
            var miles = (input / 1609.344);
            var km = (input * 1000);
            var feet = (input / 0.3048);
            var inch = (input / 0.0254);
            var cm = (input * 100);

            var embed = new EmbedBuilder
            {
                Title = $"Converted {input}Meters too",
                Description = $"Miles: {miles}miles\nKilometers: {km}km\nFeet: {feet}ft\nInches: {inch}in\nCentimeters: {cm}cm",
                Color = new Color(0xA94114)
            };

            await ReplyAsync("", embed: embed.Build());
        }

        [Command("cm")]
        [Summary("Convert cm to other")]
        public async Task Cescius(double input)
        {
            var feet = (input / 30.48);
            var miles = (input / 160934.4);
            var km = (input / 100000);
            var meter = (input / 100);
            var inch = (input / 2.54);


            var embed = new EmbedBuilder
            {
                Title = $"Converted {input}cm",
                Description = $"Feet: {feet}ft\nMiles: {miles}miles\nKilometers: {km}km\nMeters: {meter}M\nInches: {inch}in",
                Color = new Color(0xA94114)
            };

            await ReplyAsync("", embed: embed.Build());
        }
    }
}
