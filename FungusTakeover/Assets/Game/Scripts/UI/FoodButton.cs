namespace Game.UI
{
    public class FoodButton : GameUIElement
    {
        //Plays Animation when Pressed
        //Should Black out if the action is not possible

        bool _isPressable = true;

        public void TransmitData(bool data = true)
        {
            throw new System.NotImplementedException();
        }

        public void ButtonPressed()
        {
            switch (_isPressable)
            {
                case true:
                    //PressAnimation
                    break;
                case false:
                    //Cannot be pressed dumbass animation
                    break;

            }
        }

        public void ChangeState(bool isPressable)
        {
            _isPressable = isPressable;
        }


    }


}


