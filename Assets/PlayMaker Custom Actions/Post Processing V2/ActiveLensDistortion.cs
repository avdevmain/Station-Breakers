// Made by lovely Waveform
using UnityEngine.Rendering.PostProcessing;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("Post Processing V2")]
	public class ActiveLensDistortion : FsmStateAction
	{
        [ActionSection("Profile")]
        [ObjectType(typeof(PostProcessProfile))]
        public FsmObject Profile;
        [ObjectType(typeof(PostProcessVolume))]
        public FsmObject Volume;

        //[ActionSection("Enable")]
        public FsmBool ActiveEnable;
        //public FsmBool EnableValue;

        //[ActionSection("Intensity")]
        public FsmBool ActiveIntensity;
        //public FsmBool IntensityValue;

        //[ActionSection("X Multiplier")]
        public FsmBool ActiveXMultiplier;
        //public FsmBool XMultiplierValue;

        //[ActionSection("Y Multiplier")]
        public FsmBool ActiveYMultiplier;
        //public FsmBool YMultiplierValue;

        //[ActionSection("CenterX")]
        public FsmBool ActiveCenterX;
        //public FsmBool CenterXValue;

        //[ActionSection("CenterY")]
        public FsmBool ActiveCenterY;
        //public FsmBool CenterYValue;

        //[ActionSection("Scale")]
        public FsmBool ActiveScale;
        //public FsmBool ScaleValue;

        [ActionSection(" ")]
        public bool everyFrame;


        private PostProcessProfile convert;
        private PostProcessVolume convert2;

        public override void Reset()
        {
            Profile = null;
            convert = null;
            convert2 = null;

            ActiveEnable = new FsmBool { UseVariable = true };

            ActiveIntensity = new FsmBool { UseVariable = true };

            ActiveXMultiplier = new FsmBool { UseVariable = true };

            ActiveYMultiplier = new FsmBool { UseVariable = true };

            ActiveCenterX = new FsmBool { UseVariable = true };

            ActiveCenterY = new FsmBool { UseVariable = true };

            ActiveScale = new FsmBool { UseVariable = true };

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
                convert.TryGetSettings(out LensDistortion lensDistortion);

                if (!ActiveEnable.IsNone)
                    lensDistortion.active = ActiveEnable.Value;
                if (!ActiveIntensity.IsNone)
                    lensDistortion.intensity.overrideState = ActiveIntensity.Value;
                if (!ActiveXMultiplier.IsNone)
                    lensDistortion.intensityX.overrideState = ActiveXMultiplier.Value;
                if (!ActiveYMultiplier.IsNone)
                    lensDistortion.intensityY.overrideState = ActiveYMultiplier.Value;
                if (!ActiveCenterX.IsNone)
                    lensDistortion.centerX.overrideState = ActiveCenterX.Value;
                if (!ActiveCenterY.IsNone)
                    lensDistortion.centerY.overrideState = ActiveCenterY.Value;
                if (!ActiveScale.IsNone)
                    lensDistortion.scale.overrideState = ActiveScale.Value;


            }



        }
	}

}
