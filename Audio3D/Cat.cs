#region File Description
//-----------------------------------------------------------------------------
// Cat.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
#endregion

namespace Audio3D
{
    /// <summary>
    /// Entity class which moves in a circle and plays cat sounds.
    /// This uses a single-shot sound, which will stop automatically
    /// when it finishes playing. See the Dog class for an example of
    /// using a looping sound.
    /// </summary>
    class Cat : SpriteEntity
    {
        #region Fields

        // How long until we should play the next sound.
        TimeSpan timeDelay = TimeSpan.Zero;

        // Random number generator for choosing between sound variations.
        static Random random = new Random();

        // Tone Test
        public bool toneTestActive;
        SoundEffectInstance toneTestSound = null;

        // Relative Velocity Test
        public bool relativeVelocityTestActive;
        public Vector3 relativeVelocityTestCameraPosition;
        public Matrix relativeVelocityTestCameraMatrix;

        #endregion


        /// <summary>
        /// Updates the position of the cat, and plays sounds.
        /// </summary>
        public override void Update(GameTime gameTime, AudioManager audioManager)
        {
            // Move the cat in a big circle.
            double time = gameTime.TotalGameTime.TotalSeconds;
            if (toneTestActive == true)
                time *= 3;

            float dx = (float)-Math.Cos(time);
            float dz = (float)-Math.Sin(time);

            Vector3 newPosition = new Vector3(dx, 0, dz) * 6000;

            if (relativeVelocityTestActive == true)
                newPosition = relativeVelocityTestCameraPosition + Vector3.Transform(new Vector3(0, 0, -6000), relativeVelocityTestCameraMatrix);

            // Update entity position and velocity.
            Velocity = newPosition - Position;
            Position = newPosition;
            if (Velocity == Vector3.Zero)
                Forward = Vector3.Forward;
            else
                Forward = Vector3.Normalize(Velocity);

            Up = Vector3.Up;


            // If the time delay has run out, trigger another single-shot sound.
            timeDelay -= gameTime.ElapsedGameTime;

            if (timeDelay < TimeSpan.Zero)
            {
                // For variety, randomly choose between three slightly different
                // variants of the sound (CatSound0, CatSound1, and CatSound2).
                string soundName = "CatSound" + random.Next(3);

                audioManager.Play3DSound(soundName, false, this);

                timeDelay += TimeSpan.FromSeconds(1.25f);
            }

            // Tone Test
            if (toneTestActive == true)
            {
                if (toneTestSound == null)
                    toneTestSound = audioManager.Play3DSound("ToneSound", true, this);
            }
            else
            {
                if (toneTestSound != null)
                {
                    toneTestSound.Stop();
                    toneTestSound = null;
                }
            }
        }
    }
}
