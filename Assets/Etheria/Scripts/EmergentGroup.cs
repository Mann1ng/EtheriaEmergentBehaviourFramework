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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace EmergentBehaviour {
    

    public class EmergentGroup : MonoBehaviour {

        public EmergentBehaviour behaviour;
        public GameObject agentPrefab;
        public int numberOfAgents = 500;
        public float agentSpeed = 10f;
       
        public float startRadiusOfAllAgents = 100f;
        
        protected List<EmergentAgent> _agents;
        public List<EmergentAgent> Agents { get { return _agents; } }


        // Use this for initialization
        void Start() {

            if (behaviour != null)
            {
                CreateAgents(agentSpeed, behaviour);
                behaviour.Initialize(this);
            }
            else {
                Debug.LogError("No behaviour set for the Emergent Group. Please create one via the Tools Unity menu.");
            }


        }


        void CreateAgents(float speedmps, EmergentBehaviour behaviour) {

            _agents = new List<EmergentAgent>();
            for (int i = 0; i < numberOfAgents; i++)
            {
                Vector3 d = UnityEngine.Random.onUnitSphere;
                Vector3 p = transform.position + UnityEngine.Random.insideUnitSphere * startRadiusOfAllAgents;
                EmergentAgent a = EmergentAgent.NewAgent(behaviour, p, d, agentSpeed, agentPrefab);
               

                a.transform.SetParent(this.transform);
                _agents.Add(a);

            }
        }



        // Update is called once per frame
        void Update() {
            

            if (_agents == null || _agents.Count == 0) return;

            behaviour.Tick();

            for (int i = 0; i < _agents.Count; i++)
            {
                _agents[i].Tick();
            }

        }
        
               

        public List<EmergentAgent> GetNeighbours(EmergentAgent agent, float neighbourhoodRadius) {

            List<EmergentAgent> ns = new List<EmergentAgent>();

            for (int i = 0; i < _agents.Count; i++)
            {
                if (agent == _agents[i]) continue;
                if ((agent.position - _agents[i].position).magnitude <= neighbourhoodRadius) {
                    ns.Add(_agents[i]);
                }
            }
            return ns;

        }


    }







   

}
