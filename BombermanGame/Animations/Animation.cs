using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace BombermanGame {

    enum AnimationState {
        New,
        InProgress,
        Stopped
    }

    class Animation {
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

        public void Start(double animationStartTime) {
            this.animationStartTime = animationStartTime;
            State = AnimationState.InProgress;
            // Set initial frame
            Update(animationStartTime);
        }

        public void Update(double currentTime) {
            if (State != AnimationState.InProgress) {
                return;
            }
            var timeSinceStart = currentTime - animationStartTime;
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

        public void Stop() {
            this.State = AnimationState.Stopped;
            this.CurrentFrame = spriteSequence[0];
        }
    }
}
