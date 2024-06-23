using System.Collections.Generic;

//布鞋--增加 10% 移动速度
public class Shoe : Equipment
{
    private int mMoveSpeedAddition=10;
    public Shoe():base(EquipmentType.Passive,EquipmentId.Shoe)
    {

    }

    public override void OnGet(StatusEffect statusEffect, int layer)
    {
        Dictionary<Property,float> corrections=new Dictionary<Property,float>();
        corrections.Add(Property.MoveSpeedAddition,mMoveSpeedAddition*layer);
        statusEffect.SetPropertyCorrections(corrections);
    }
}