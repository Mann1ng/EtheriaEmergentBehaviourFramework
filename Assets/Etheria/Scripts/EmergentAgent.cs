//
// Etheria Emergent Behaviour Framework.
//
// Copyright (C) 2018 Isaac Dart (www.linkedin.com/in/isaacdart)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using UnityEngine;


namespace EmergentBehaviour
{
   
    public class EmergentAgent 
    {
        public EmergentBehaviour currentBehaviour = null;
        public Vector3 position;
        public Vector3 direction;
        public float speedMetersPerSecond = 5f;
        public float neighbourhoodRadius = 80;

        public Transform transform = null;

        public static EmergentAgent NewAgent( EmergentBehaviour behaviour)
        {
            EmergentAgent a = new EmergentAgent();
            a.currentBehaviour = behaviour;
            a.position = Vector3.zero;
            a.direction = Vector3.zero;
            return a;
        }

        public static EmergentAgent NewAgent(EmergentBehaviour behaviour, Vector3 position, Vector3 direction, float speedMetersPerSecond, GameObject prefab)
        {
            EmergentAgent a = new EmergentAgent();
            a.currentBehaviour = behaviour;

            GameObject g = GameObject.Instantiate<GameObject>(prefab, position, Quaternion.identity);
            a.transform = g.transform;
            a.position = position;
            a.direction = direction;
            a.speedMetersPerSecond = speedMetersPerSecond;
            

            return a;

        }

        public void Tick() {
            if (this.currentBehaviour != null) { currentBehaviour.UpdateAgent(this); }
        }

    
      

    }

}
