﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunny.DAL;
using Sunny.Model;

namespace Sunny.BLL.API
{
    public class ClassBLL
    {
        //下单前应该检查是否已经购买过本课程，同一微信号不能重复购买

        public static bool Booking(int studentid, int productId, DateTime start_time)
        {
            Course course = DBData.GetInstance(DBTable.course).GetEntity<Course>($"student_id='{studentid}' and product_id='{productId}'");
            Class data = new Class()
            {
                product_id = productId,
                coach_id = 0,
                venue_id = course.venue_id,
                hour = course.over_hour + 1,
                max_count = course.max_count,
                start_time = start_time,
                end_time = start_time.AddHours(1),
            };
            return ClassDAL.InsertClassData(data);
        }


    }
}