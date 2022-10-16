// Made by lovely Waveform
using UnityEngine.Rendering.PostProcessing;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("Post Processing V2")]
	public class GetActiveScreenSpaceReflections : FsmStateAction
	{
        [ActionSection("Profile")]
        [ObjectType(typeof(PostProcessProfile))]
        public FsmObject Profile;
        [ObjectType(typeof(PostProcessVolume))]
        public FsmObject Volume;
        [UIHint(UIHint.Variable)]
        [ObjectType(typeof(PostProcessVolume))]
        public FsmObject VolumeProfile;

        //[ActionSection("Enable")]
        //public FsmBool GetEnable;
        public FsmBool EnableValue;

        //[ActionSection("Preset")]
        //public FsmBool GetPreset;
        [UIHint(UIHint.Variable)]
        public FsmBool PresetValue;

        //[ActionSection("Max March Distance")]
        //public FsmBool GetMaxMarchDistance;
        [UIHint(UIHint.Variable)]
        public FsmBool MaxMarchDistanceValue;

        //[ActionSection("Distance Fade")]
        //public FsmBool GetDistanceFade;
        [UIHint(UIHint.Variable)]
        public FsmBool DistanceFadeValue;

        //[ActionSection("Vignette")]
        //public FsmBool GetVignette;
        [UIHint(UIHint.Variable)]
        public FsmBool VignetteValue;

        [ActionSection(" ")]
        public bool everyFrame;


        private PostProcessProfile convert;
        private PostProcessVolume convert2;

        public override void Reset()
        {
            Profile = null;
            convert = null;
            convert2 = null;
            /*
            GetEnable = false;
            GetPreset = false;
            GetMaxMarchDistance = false;
            GetDistanceFade = false;
            GetVignette = false;
            */
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
                VolumeProfile.Value = convert;
            }
            if (convert == null)
            {
                return;
            }
            else
            {
                convert.TryGetSettings(out ScreenSpaceReflections screenSpaceReflections);

                if (!EnableValue.IsNone)
                    EnableValue.Value=screenSpaceReflections.active;
                if (!PresetValue.IsNone)
                    PresetValue.Value=screenSpaceReflections.preset.overrideState;
                if (!MaxMarchDistanceValue.IsNone)
                    MaxMarchDistanceValue.Value=screenSpaceReflections.maximumMarchDistance.overrideState;
                if (!DistanceFadeValue.IsNone)
                    DistanceFadeValue.Value=screenSpaceReflections.distanceFade.overrideState;
                if (!VignetteValue.IsNone)
                    VignetteValue.Value=screenSpaceReflections.vignette.overrideState;

            }



        }
	}

}
