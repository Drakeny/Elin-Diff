﻿using System;
using System.Collections.Generic;

public class Los : EClass
{
	public static int MAX(int a, int b)
	{
		if (a <= b)
		{
			return b;
		}
		return a;
	}

	public static int ABS(int a)
	{
		if (a >= 0)
		{
			return a;
		}
		return -a;
	}

	public static int ZSGN(int a)
	{
		if (a < 0)
		{
			return -1;
		}
		if (a <= 0)
		{
			return 0;
		}
		return 1;
	}

	private static bool IsBlocked(int x, int z, int sx, int sz)
	{
		bool flag = false;
		if (x < 0 || z < 0 || x >= EClass._map.Size || z >= EClass._map.Size)
		{
			flag = true;
		}
		else
		{
			Cell cell = EClass._map.cells[x, z];
			byte[] weights = EClass._map.cells[x, z].weights;
			if ((sz == -1 && weights[0] == 0) || (sz == 1 && weights[2] == 0) || (sx == 1 && weights[1] == 0) || (sx == -1 && weights[3] == 0))
			{
				flag = true;
			}
			if (!flag && sx != 0 && sz != 0)
			{
				if (x + sx < 0 || z + sz < 0 || x + sx >= EClass._map.Size || z + sz >= EClass._map.Size)
				{
					flag = true;
				}
				else
				{
					byte[] weights2 = EClass._map.cells[x + sx, z + sz].weights;
					if ((sz < 0 && weights2[2] == 0) || (sx > 0 && weights2[3] == 0))
					{
						flag = true;
					}
					if ((sz > 0 && weights2[0] == 0) || (sx < 0 && weights2[1] == 0))
					{
						flag = true;
					}
				}
			}
			if (cell.blockSight)
			{
				flag = true;
			}
		}
		Los.p.Set(x, z);
		if (Los.onVisit != null)
		{
			Los.onVisit(Los.p, flag);
		}
		return flag;
	}

	public static List<Point> ListVisible(Point p1, Point p2, int radius, Action<Point, bool> _onVisit = null)
	{
		List<Point> list = new List<Point>();
		List<Point> vecs = new List<Point>();
		Point lastPoint = p1.Copy();
		Los.IsVisible(p1.x, p2.x, p1.z, p2.z, delegate(Point p, bool blocked)
		{
			Point point3 = new Point(p.x - lastPoint.x, p.z - lastPoint.z);
			if (point3.x != 0 || point3.z != 0)
			{
				vecs.Add(point3);
			}
			lastPoint.Set(p);
		}, false);
		if (vecs.Count == 0)
		{
			return list;
		}
		Point point = p1.Copy();
		for (int i = 0; i < radius; i++)
		{
			Point point2 = vecs[i % vecs.Count];
			point.x += point2.x;
			point.z += point2.z;
		}
		Los.IsVisible(p1.x, point.x, p1.z, point.z, delegate(Point p, bool blocked)
		{
			if (!blocked)
			{
				list.Add(p.Copy());
			}
			if (_onVisit != null)
			{
				_onVisit(p, blocked);
			}
		}, true);
		return list;
	}

	public static Point GetNearestNeighbor(Point p1, Point p2)
	{
		Point dest = null;
		Los.IsVisible(p1, p2, delegate(Point p, bool blocked)
		{
			if (!blocked && dest == null && p2.Distance(p) == 1 && !p.HasChara)
			{
				dest = new Point(p);
			}
		});
		return dest;
	}

	public static Point GetRushPoint(Point p1, Point dest)
	{
		Point rushPos = null;
		bool valid = true;
		Los.IsVisible(p1, dest, delegate(Point p, bool blocked)
		{
			if (p.Equals(dest) || p.Equals(p1))
			{
				return;
			}
			if (blocked || p.HasChara || p.IsBlocked)
			{
				valid = false;
			}
			if (p.Distance(dest) == 1)
			{
				rushPos = p.Copy();
			}
		});
		if (!valid)
		{
			return null;
		}
		return rushPos;
	}

	public static bool IsVisible(Point p1, Point p2, Action<Point, bool> _onVisit = null)
	{
		return Los.IsVisible(p1.x, p2.x, p1.z, p2.z, _onVisit, true);
	}

	public static bool IsVisible(Card c1, Card c2)
	{
		return Los.IsVisible(c1.pos.x, c2.pos.x, c1.pos.z, c2.pos.z, null, true);
	}

	public static bool IsVisible(int x1, int x2, int z1, int z2, Action<Point, bool> _onVisit = null, bool returnOnBlock = true)
	{
		Los.onVisit = _onVisit;
		Los.p.Set(x1, z1);
		Los.originalP.Set(x1, z1);
		int a = x2 - x1;
		int a2 = z2 - z1;
		int num = Los.ABS(a) << 1;
		int num2 = Los.ABS(a2) << 1;
		int num3 = Los.ZSGN(a);
		int num4 = Los.ZSGN(a2);
		int num5 = x1;
		int num6 = z1;
		if (num >= num2)
		{
			int num7 = num2 - (num >> 1);
			while (num5 != x2)
			{
				if (Los.IsBlocked(num5, num6, num3, (num7 >= 0) ? num4 : 0) && returnOnBlock)
				{
					return false;
				}
				if (num7 >= 0)
				{
					num6 += num4;
					num7 -= num;
				}
				num5 += num3;
				num7 += num2;
			}
			Los.p.Set(num5, num6);
			if (Los.onVisit != null)
			{
				Los.onVisit(Los.p, false);
			}
			return true;
		}
		if (num2 >= num)
		{
			int num8 = num - (num2 >> 1);
			while (num6 != z2)
			{
				if (Los.IsBlocked(num5, num6, (num8 >= 0) ? num3 : 0, num4) && returnOnBlock)
				{
					return false;
				}
				if (num8 >= 0)
				{
					num5 += num3;
					num8 -= num2;
				}
				num6 += num4;
				num8 += num;
			}
			Los.p.Set(num5, num6);
			if (Los.onVisit != null)
			{
				Los.onVisit(Los.p, false);
			}
			return true;
		}
		return false;
	}

	public static Point p = new Point();

	public static Point originalP = new Point();

	public static Action<Point, bool> onVisit;
}
