﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace BombermanGame {

    enum AnimationState {
        New,
        InProgress,
        Stopped
    }

    class Animation : IUpdateable {
        private readonly IReadOnlyList<Bitmap> spriteSequence = new List<Bitmap>();
        private double cycleDuration;
        private bool repeatCycle;
        private double animationStartTime;

        public Animation(IEnumerable<Bitmap> spriteSequence, double cycleDuration, bool repeatCycle) {
            this.spriteSequence = spriteSequence.ToList();
            this.cycleDuration = cycleDuration;
            this.repeatCycle = repeatCycle;
            CurrentFrame = spriteSequence.FirstOrDefault();
        }

        public AnimationState State { get; private set; }
        public Bitmap CurrentFrame { get; private set; }

        public void start(double animationStartTime) {
            this.animationStartTime = animationStartTime;
            State = AnimationState.InProgress;
            // Set initial frame
            update(0, animationStartTime);
        }

        public void update(double tick, double totalTime) {
            if (State != AnimationState.InProgress) {
                return;
            }
            var timeSinceStart = totalTime - animationStartTime;
            var timePerSprite = cycleDuration / this.spriteSequence.Count;
            var nextFrameIndex = (int)Math.Floor(timeSinceStart / timePerSprite);

            if (this.repeatCycle) {
                // Make sure we restart from 0 when the animation cycle has ended
                nextFrameIndex %= spriteSequence.Count;
            }

            if (nextFrameIndex >= spriteSequence.Count) {
                this.State = AnimationState.Stopped;
                this.CurrentFrame = null;
            } else {
                this.CurrentFrame = spriteSequence[nextFrameIndex];
            }
        }

        public void stop() {
            this.State = AnimationState.Stopped;
            this.CurrentFrame = spriteSequence[0];
        }
    }
}
