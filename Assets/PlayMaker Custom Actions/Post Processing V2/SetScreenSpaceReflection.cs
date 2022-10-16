// Made by lovely Waveform
using UnityEngine.Rendering.PostProcessing;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("Post Processing V2")]
	public class SetScreenSpaceReflections : FsmStateAction
	{
        [ActionSection("Profile")]
        [ObjectType(typeof(PostProcessProfile))]
        public FsmObject Profile;
        [ObjectType(typeof(PostProcessVolume))]
        public FsmObject Volume;

        [ActionSection("Enable")]
        public FsmBool SetEnable;
        public FsmBool EnableValue;

        [ActionSection("Preset")]
        public FsmBool SetPreset;
        [ObjectType(typeof(ScreenSpaceReflectionPreset))]
        public FsmEnum PresetValue;

        [ActionSection("Max March Distance")]
        public FsmBool SetMaxMarchDistance;
        public FsmFloat MaxMarchDistanceValue;

        [ActionSection("Distance Fade")]
        public FsmBool SetDistanceFade;
        [HasFloatSlider(0,1)]
        public FsmFloat DistanceFadeValue;

        [ActionSection("Vignette")]
        public FsmBool SetVignette;
        [HasFloatSlider(0, 1)]
        public FsmFloat VignetteValue;

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

            SetPreset = false;
            PresetValue = ScreenSpaceReflectionPreset.Medium;

            SetMaxMarchDistance = false;
            MaxMarchDistanceValue = 100;

            SetDistanceFade = false;
            DistanceFadeValue = 0.5f;

            SetVignette = false;
            VignetteValue = 0.5f;

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
                convert.TryGetSettings(out ScreenSpaceReflections screenSpaceReflections);

                if (SetEnable.Value)
                    screenSpaceReflections.enabled.value = EnableValue.Value;
                if (SetPreset.Value)
                    screenSpaceReflections.preset.value = (ScreenSpaceReflectionPreset)PresetValue.Value;
                if (SetMaxMarchDistance.Value)
                    screenSpaceReflections.maximumMarchDistance.value = MaxMarchDistanceValue.Value;
                if (SetDistanceFade.Value)
                    screenSpaceReflections.distanceFade.value = DistanceFadeValue.Value;
                if (SetVignette.Value)
                    screenSpaceReflections.vignette.value = VignetteValue.Value;

            }



        }
	}

}
