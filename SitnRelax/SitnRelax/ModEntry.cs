using System;
using System.IO;
using System.Threading;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using Newtonsoft.Json;
using StardewValley.Monsters;

namespace SitnRelax
{
    public class ModEntry : Mod
    {
        
        private int SecondsUntilRegen;
        private bool initialSit = false;
        private configModel Config;
        //public PerScreen<int> sittingHP;

        
        public override void Entry(IModHelper helper)
        {
            helper.Events.GameLoop.UpdateTicked += this.UpdateTicked;
           // helper.Events.Input.ButtonPressed += this.OnButtonPressed;
            this.Config = helper.ReadConfig<configModel>();
            
        }
        /*private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button == this.Config.sitKey && this.Config.allowSitAnywhere == true)
            {
                Monitor.Log("Sit Anywhere was used!", LogLevel.Info);
                if (!Game1.player.isSitting)
                {
                    

                }

                else if (Game1.player.isSitting.Value == true)
                {
                    Game1.player.isStopSitting = true;
                    
                }
            }
        }*/
        private void UpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            if (!Game1.player.isSitting)
            {
                this.SecondsUntilRegen = this.Config.regenRate;
                initialSit = false;
                return;
            }

            // wait until regen time
            if (this.SecondsUntilRegen > 0)
            {
                if (initialSit == false)
                {
                    Game1.addHUDMessage(new HUDMessage($"{Game1.player.name} {Config.restMessage}", 4));
                   // sittingHP = Game1.player.health;
                    initialSit = true;
                }

                if (e.IsOneSecond)
                    this.SecondsUntilRegen--;

                if (this.SecondsUntilRegen > 0)
                    return;
            }
            //apply regen 
            if (Game1.player.stamina >= Game1.player.maxStamina.Value && Game1.player.health >= Game1.player.maxHealth)
            {
                Monitor.Log("Stamina and Health are full.. doing nothing...", LogLevel.Trace);

            }
            else if (Game1.player.stamina < Game1.player.maxStamina.Value)
                {
                    Game1.player.stamina = Math.Min(Game1.player.stamina + this.Config.stamRegen, Game1.player.maxStamina.Value);
                    Monitor.Log("Regenerating Stamina", LogLevel.Trace);
                    SecondsUntilRegen = this.Config.regenRate;

                }
            else if (Game1.player.stamina >= Game1.player.maxStamina.Value)
            {
                    if (Game1.player.health < Game1.player.maxHealth)
                    {
                        Game1.player.health = Game1.player.health + this.Config.healthRegen;
                        Monitor.Log("Stamina Full... Regenerating Health", LogLevel.Trace);
                        SecondsUntilRegen = this.Config.regenRate;
                    }
            }
            /*if (Game1.player.health < sittingHP)
            {
                double damageTaken = sittingHP - Game1.player.health;
                double addDamage = damageTaken * .50;
                Game1.player.takeDamage((int)addDamage, false, null);
                Game1.addHUDMessage(new HUDMessage("Hey!! What are you doing?! Don't let your guard down!!", 3));
                Game1.player.isStopSitting = true;
                Game1.player.currentTemporaryInvincibilityDuration = 0;
                sittingHP = Game1.player.health;
            }*/

            
        }
    }
}
