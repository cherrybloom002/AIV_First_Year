using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankz_2023
{
    enum StateEnum
    {
        WAIT, PLAY, SHOOT
    }

    class StateMachine
    {
        private Dictionary<StateEnum, State> states;
        private State current;

        public StateMachine()
        {
            states = new Dictionary<StateEnum, State>();
            current = null;
        }

        public void AddState(StateEnum key, State value)
        {
            states[key] = value;
            value.SetStateMachine(this);
        }

        public void GoTo(StateEnum key)
        {
            if (current != null)
            {
                current.OnExit();
            }

            current = states[key];
            current.OnEnter();
        }

        public void Update()
        {
            if (current != null)
                current.Update();
        }
    }
}
