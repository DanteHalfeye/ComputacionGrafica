using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class MaterialFloatPlayableAsset : PlayableAsset
{
    public ExposedReference<Material> targetMaterial;
    public string propertyName;
    public float startValue = 0f;
    public float endValue = 1f;
    public bool playOnce = true;
    public bool playReverse = false;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<MaterialFloatPlayableBehaviour>.Create(graph);
        var behaviour = playable.GetBehaviour();

        behaviour.propertyName = propertyName;
        behaviour.startValue = startValue;
        behaviour.endValue = endValue;
        behaviour.playOnce = playOnce;
        behaviour.playReverse = playReverse;

        // Resolve the exposed material reference from the director context
        Material mat = targetMaterial.Resolve(graph.GetResolver());
        behaviour.SetMaterial(mat);

        return playable;
    }
}