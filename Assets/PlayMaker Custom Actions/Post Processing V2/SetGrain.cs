// Made by lovely Waveform
using UnityEngine.Rendering.PostProcessing;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("Post Processing V2")]
	public class SetGrain : FsmStateAction
	{
        [ActionSection("Profile")]
        [ObjectType(typeof(PostProcessProfile))]
        public FsmObject Profile;
        [ObjectType(typeof(PostProcessVolume))]
        public FsmObject Volume;

        [ActionSection("Enable")]
        public FsmBool SetEnable;
        public FsmBool EnableValue;

        [ActionSection("Colored")]
        public FsmBool SetColored;
        public FsmBool ColoredValue;

        [ActionSection("Intensity")]
        public FsmBool SetIntensity;
        [HasFloatSlider(0,1)]
        public FsmFloat IntensityValue;

        [ActionSection("Size")]
        public FsmBool SetSize;
        [HasFloatSlider(0.3f,3)]
        public FsmFloat SizeValue;

        [ActionSection("Luminance Contribution")]
        public FsmBool SetLuminanceContribution;
        [HasFloatSlider(0, 1)]
        public FsmFloat LuminanceContributionValue;

        [ActionSection(" ")]
        public bool everyFrame;


        private PostProcessProfile convert;
        private PostProcessVolume convert2;

        public override void Reset()
        {
            Profile = null;
            convert = null;
            convert2 = null;

            SetEnable = false;
            EnableValue = false;

            SetColored = false;
            ColoredValue = true;

            SetIntensity = false;
            IntensityValue = 0;

            SetSize = false;
            SizeValue = 1;

            SetLuminanceContribution = false;
            LuminanceContributionValue = 0.8f;

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

                if (SetEnable.Value)
                    grain.enabled.value = EnableValue.Value;
                if (SetColored.Value)
                    grain.colored.value = ColoredValue.Value;
                if (SetIntensity.Value)
                    grain.intensity.value = IntensityValue.Value;
                if (SetSize.Value)
                    grain.size.value = SizeValue.Value;
                if (SetLuminanceContribution.Value)
                    grain.lumContrib.value = LuminanceContributionValue.Value;



            }



        }
	}

}
