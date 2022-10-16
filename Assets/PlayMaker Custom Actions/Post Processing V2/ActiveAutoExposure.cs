// Made by lovely Waveform
using UnityEngine.Rendering.PostProcessing;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("Post Processing V2")]
	public class ActiveAutoExposure : FsmStateAction
	{
        [ActionSection("Profile")]
        [ObjectType(typeof(PostProcessProfile))]
        public FsmObject Profile;
        [ObjectType(typeof(PostProcessVolume))]
        public FsmObject Volume;

        //[ActionSection("Root")]
        public FsmBool ActiveRoot;
        //public FsmBool RootValue;

        //[ActionSection("Filtering")]
        public FsmBool ActiveFiltering;
        //public FsmBool FilteringValue;

        //[ActionSection("MinimumEV")]
        public FsmBool ActiveMinimumEV;
        //public FsmBool MinimumEVValue;

        //[ActionSection("MaximumEV")]
        public FsmBool ActiveMaximumEV;
        //public FsmBool MaximumEVValue;
        /*
        [ActionSection("Compensation")]
        public FsmBool ActiveCompensation;
        public FsmFloat CompensationValue;
        */
        //[ActionSection("Adaption Type")]
        public FsmBool ActiveType;
        //public FsmBool TypeValue;

        //[ActionSection("Speed Up")]
        public FsmBool ActiveSpeedUp;
        //public FsmBool SpeedUpValue;

        //[ActionSection("Speed Down")]
        public FsmBool ActiveSpeedDown;
        //public FsmBool SpeedDownValue;

        [ActionSection(" ")]
        public bool everyFrame;


        private PostProcessProfile convert;
        private PostProcessVolume convert2;

        public override void Reset()
        {
            Profile = null;
            convert = null;
            convert2 = null;
            
            ActiveRoot = new FsmBool { UseVariable = true };

            ActiveFiltering = new FsmBool { UseVariable = true };

            ActiveMinimumEV = new FsmBool { UseVariable = true };

            ActiveMaximumEV = new FsmBool { UseVariable = true };
            ActiveType = new FsmBool { UseVariable = true };

            ActiveSpeedUp = new FsmBool { UseVariable = true };

            ActiveSpeedDown = new FsmBool { UseVariable = true };
            
            everyFrame = false;

        }
        public override void OnEnter()
		{

            ggop();

            if(!everyFrame)
            {
                Finish();
            }

        }

        public override void OnUpdate()
        {
            ggop();
        }
        private void ggop()
        {
            if (Profile.Value != null)
            {
                convert = (PostProcessProfile)Profile.Value;
            }
            else if (Volume.Value != null)
            {
                convert2 = (PostProcessVolume)Volume.Value;
                convert = convert2.profile;
            }
            if (convert == null)
            {
                return;
            }
            else
            {
                convert.TryGetSettings(out AutoExposure autoExposure);

                if (!ActiveRoot.IsNone)
                    autoExposure.active = ActiveRoot.Value;
                if (!ActiveFiltering.IsNone)
                    autoExposure.filtering.overrideState = ActiveFiltering.Value;
                if (!ActiveMinimumEV.IsNone)
                    autoExposure.minLuminance.overrideState = ActiveMinimumEV.Value;
                if (!ActiveMaximumEV.IsNone)
                    autoExposure.maxLuminance.overrideState = ActiveMaximumEV.Value;
                if (!ActiveType.IsNone)
                    autoExposure.eyeAdaptation.overrideState = ActiveType.Value;
                if (!ActiveSpeedUp.IsNone)
                    autoExposure.speedUp.overrideState = ActiveSpeedUp.Value;
                if (!ActiveSpeedDown.IsNone)
                    autoExposure.speedDown.overrideState = ActiveSpeedDown.Value;

            }



        }
	}

}
