using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export] public float Kecepatan = 100;

	[Export] public int LantaiSekarang = 1;     // 1–4 lantai
	private const int Z_PER_LANTAI = 2;         // jarak Z tiap lantai

	private AnimatedSprite2D animasi;

	public override void _Ready()
	{
		animasi = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		// Biar z_index global, bukan relatif ke parent
		ZAsRelative = false;
		UpdateZIndex();
	}

	private void UpdateZIndex()
	{
		// Lantai 1  -> z = 1  (di antara Lantai1[0] dan Lantai2[2])
		// Lantai 2  -> z = 3  (di antara Lantai2[2] dan Lantai3[4])
		// Lantai 3  -> z = 5  (di antara Lantai3[4] dan Lantai4[6])
		// Lantai 4  -> z = 7  (di atas semua)
		ZIndex = LantaiSekarang * Z_PER_LANTAI - 1;
	}
	
	public void SetLantai(int lantaiBaru)
	{
		LantaiSekarang = lantaiBaru;
		UpdateZIndex();
	}

	// 🟢 TAMBAHKAN FUNGSI INI
	private void _on_tangga_1_ke_2_body_entered(Node2D body)
{
	if (body == this && LantaiSekarang == 1)
	{
		SetLantai(2);
	}
}

	// 🟢 SAMPAI SINI
	private void _on_tangga_2_ke_3_body_entered(Node2D body)
{
	if (body == this && LantaiSekarang == 2)
	{
		SetLantai(3);
	}
}
	private void _on_tangga_3_ke_4_body_entered(Node2D body)
{
	if (body == this && LantaiSekarang == 3)
	{
		SetLantai(4);
	}
}
	private void _on_tangga_2_ke_1_body_entered(Node2D body)
{
	if (body == this && LantaiSekarang == 2)
	{
		SetLantai(1);
	}
}
	private void _on_tangga_3_ke_2_body_entered(Node2D body)
{
	if (body == this && LantaiSekarang == 3)
	{
		SetLantai(2);
	}
}
	private void _on_tangga_4_ke_3_body_entered(Node2D body)
{
	if (body == this && LantaiSekarang == 4)
	{
		SetLantai(3);
	}
}



	public override void _PhysicsProcess(double delta)
	{
		Vector2 arahGerak = Vector2.Zero;

		if (Input.IsActionPressed("gerak_kanan")) arahGerak.X += 1;
		if (Input.IsActionPressed("gerak_kiri"))  arahGerak.X -= 1;
		if (Input.IsActionPressed("gerak_bawah")) arahGerak.Y += 1;
		if (Input.IsActionPressed("gerak_atas"))  arahGerak.Y -= 1;

		Velocity = arahGerak.Normalized() * Kecepatan;
		MoveAndSlide();

		if (arahGerak != Vector2.Zero)
		{
			if (Math.Abs(arahGerak.X) > Math.Abs(arahGerak.Y))
			{
				animasi.Play("jalan_kiri");
				animasi.FlipH = arahGerak.X > 0;
			}
			else
			{
				animasi.Play(arahGerak.Y > 0 ? "jalan_bawah" : "jalan_atas");
			}
		}
		else
		{
			animasi.Stop();
		}
	}
}
