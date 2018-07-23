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
using System.Collections.Generic;
using UnityEngine;

namespace EmergentBehaviour
{
    
    public class FlockingBehaviour : EmergentBehaviour
    {

        public float separationDistance = 10f;
        public float windMetersPerSecond = 10f;
       
        public float radiusOfNeighbourhood = 30f;
        public float velocityVariation = 7f;
        public int minGroupSize = 2;
        public float universalCohesionBias = 0.25f;
        

        public float alignmentWeight = 1f;
        public float cohesionWeight = 1f;
        public float separationWeight = 1f;

        protected float universalCohesion = 0.25f;
        
        protected Vector3 windDirection = default(Vector3);
        




        public override void Initialize(EmergentGroup group)
        {
            base.Initialize(group);
            Name = "Flocking";
           
        }
        

        public override void UpdateAgent(EmergentAgent a)
        {

            List<EmergentAgent> ns = _group.GetNeighbours(a, radiusOfNeighbourhood);

            
            if (ns.Count > minGroupSize)
            {
                universalCohesion = Mathf.Sin(Time.time) + universalCohesionBias;

                Vector3 groupAlignment = Vector3.zero; 
                Vector3 groupCenter = _group.transform.position;
                Vector3 groupSeparation = default(Vector3);
                Vector3 groupCohesion = default(Vector3);


                for (int i = 0; i < ns.Count; i++)
                {
                    groupAlignment += ns[i].direction;
                    groupCenter += ns[i].position;
                    groupSeparation += CalcSeparation(a, ns[i]);
                }

                //Alignment: Average all neighbours direction
                groupAlignment /= ns.Count;
                groupAlignment.Normalize();


                //Cohesion: Direction towards the center of mass
                groupCenter /= ns.Count;
                groupCenter += (_group.transform.position - groupCenter) * universalCohesion;
                groupCohesion = (groupCenter - a.position).normalized;

                a.direction +=
                       groupAlignment * Time.deltaTime * alignmentWeight
                     + groupCohesion * Time.deltaTime * cohesionWeight
                     + groupSeparation * Time.deltaTime * separationWeight;

                a.direction.Normalize();

            }
            else
            {
               //if we don't have enough friends then make a sad and introspective journey towards the center of the universe..
                a.direction = (_group.transform.position - a.position).normalized;
            }
            
           
            a.position += a.direction * a.speedMetersPerSecond * Time.deltaTime;
            
            if (a.transform != null) a.transform.position = a.position;
                   
        }

        Vector3 CalcSeparation(EmergentAgent agent, EmergentAgent other)
        {
            Vector3 heading = agent.position - other.position;
            if (heading.magnitude < separationDistance)
            {
                float scale = heading.magnitude / separationDistance;
                return heading.normalized / scale;
            }
            else {
                return Vector3.zero;
            }
        }



        
        public override void Tick()
        {
            
            //add some motion to the center of the entire universe...
            windDirection = new Vector3(
                Mathf.PerlinNoise(Time.time, windMetersPerSecond) * 2.0f - 1.0f,
                Mathf.PerlinNoise(Time.time, windMetersPerSecond) * 2.0f - 1.0f,
                Mathf.PerlinNoise(Time.time, windMetersPerSecond) * 2.0f - 1.0f
                ) ;

            _group.transform.position += windDirection * velocityVariation * Time.deltaTime;
            
        }


        /// <summary>
        /// Called from menu when creating a new flocking scriptable asset
        /// </summary>
        /// <returns></returns>
        public static FlockingBehaviour CreateFlockingBehaviour()
        {
            FlockingBehaviour fb = ScriptableObject.CreateInstance<FlockingBehaviour>();
            return fb;
        }




    }

}
