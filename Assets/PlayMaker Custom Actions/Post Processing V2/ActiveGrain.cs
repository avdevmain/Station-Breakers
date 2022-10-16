// Made by lovely Waveform
using UnityEngine.Rendering.PostProcessing;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("Post Processing V2")]
	public class ActiveGrain : FsmStateAction
	{
        [ActionSection("Profile")]
        [ObjectType(typeof(PostProcessProfile))]
        public FsmObject Profile;
        [ObjectType(typeof(PostProcessVolume))]
        public FsmObject Volume;

        //[ActionSection("Enable")]
        public FsmBool ActiveEnable;
        //public FsmBool EnableValue;

        //[ActionSection("Colored")]
        public FsmBool ActiveColored;
        //public FsmBool ColoredValue;

        //[ActionSection("Intensity")]
        public FsmBool ActiveIntensity;
        //public FsmBool IntensityValue;

        //[ActionSection("Size")]
        public FsmBool ActiveSize;
        //public FsmBool SizeValue;

        //[ActionSection("Luminance Contribution")]
        public FsmBool ActiveLuminanceContribution;
        //public FsmBool LuminanceContributionValue;

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

            ActiveColored = new FsmBool { UseVariable = true };

            ActiveIntensity = new FsmBool { UseVariable = true };

            ActiveSize = new FsmBool { UseVariable = true };

            ActiveLuminanceContribution = new FsmBool { UseVariable = true };

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
                convert.TryGetSettings(out Grain grain);

                if (!ActiveEnable.IsNone)
                    grain.active = ActiveEnable.Value;
                if (!ActiveColored.IsNone)
                    grain.colored.overrideState = ActiveColored.Value;
                if (!ActiveIntensity.IsNone)
                    grain.intensity.overrideState = ActiveIntensity.Value;
                if (!ActiveSize.IsNone)
                    grain.size.overrideState = ActiveSize.Value;
                if (!ActiveLuminanceContribution.IsNone)
                    grain.lumContrib.overrideState = ActiveLuminanceContribution.Value;



            }



        }
	}

}
