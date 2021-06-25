using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace HackathonUCL
{
    public class SceneManager
    {
        internal readonly List<Scene> scenes = new List<Scene>();

        public int CurrentSceneIndex { get; set; }

        public Scene CurrentScene => scenes[CurrentSceneIndex];

        public void NextScene() { if (CurrentSceneIndex < scenes.Count) CurrentSceneIndex++; }

        public void PreviousScene() { if (CurrentSceneIndex > 0) CurrentSceneIndex--; }

        public void Update()
        {
            CurrentScene.Update();
            if (CurrentScene.Finished)
                CurrentSceneIndex++;
        }

        public void Draw(SpriteBatch spriteBatch) =>
            CurrentScene.Draw(spriteBatch);

        public static SceneManager Instance;

        public SceneManager(List<Scene> scenes)
        {
            this.scenes = scenes;
            Instance = this;
        }
    }
}
