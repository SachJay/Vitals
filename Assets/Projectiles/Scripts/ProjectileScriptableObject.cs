using UnityEngine;

[CreateAssetMenu(fileName = "Projectile", menuName = "Scriptable Object/Projectile")]
public class ProjectileScriptableObject : ScriptableObject
{
    [SerializeField] private ProjectileType projectileType;
    [SerializeField] private Color projectileColor = Color.white;
    [SerializeField] private Vector3 projectileScale = Vector3.one;
    [SerializeField] private float projectileSpeed = 1;
    [SerializeField] private float projectileDuration = 1;
    [SerializeField] private bool isContactDamage = true;

    public ProjectileType ProjectileType => projectileType;
    public Color ProjectileColor => projectileColor;
    public Vector3 ProjectileScale => projectileScale;
    public float ProjectileSpeed => projectileSpeed;
    public float ProjectileDuration => projectileDuration;
    public bool IsContactDamage => isContactDamage;
}
