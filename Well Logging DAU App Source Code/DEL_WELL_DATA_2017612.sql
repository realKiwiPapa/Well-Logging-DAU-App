create or replace 
PROCEDURE "DEL_WELL_DATA"( WellID in CHAR,p_cursor OUT SYS_REFCURSOR)
AS
  V_DRILL_JOB_ID varchar2(32);
  V_REQUISITION_CD varchar2(100);
  V_PROCESS_ID varchar2(100);
  V_FILEID number(38,0);
  V_FILEID_1 number(38,0);
  V_UploadID number(38,0);
  V_Count number(38,0);
BEGIN
    select DRILL_ENG_DES_FILEID,DRILL_GEO_DES_FILEID into V_FILEID,V_FILEID_1 from COM_WELL_BASIC where WELL_ID=WellID;
    if V_FILEID is not null then 
        insert into well_del_temp_tbl(fileid) values(V_FILEID);
    end if;
    if V_FILEID_1 is not null then
        insert into well_del_temp_tbl(fileid) values(V_FILEID_1);
    end if;
  --定义游标
    declare cursor cursor_1 is select DRILL_JOB_ID from COM_JOB_INFO where WELL_ID=WellID;
    begin
      open cursor_1;
    loop  
      fetch cursor_1 into V_DRILL_JOB_ID;
      exit when cursor_1%notfound;
        declare cursor cursor_2 is select REQUISITION_CD,REQUISITION_SCANNING_FILEID from DM_LOG_TASK where DRILL_JOB_ID=V_DRILL_JOB_ID;
        begin
        open cursor_2;
        loop
        fetch cursor_2 into V_REQUISITION_CD,V_FILEID;
        exit when cursor_2%notfound;
           if V_FILEID is not null then
              insert into well_del_temp_tbl(fileid) values(V_FILEID); 
           end if;
           declare cursor cursor_21 is select PLAN_CONTENT_SCANNING_FILEID from DM_LOG_OPS_PLAN  where REQUISITION_CD=V_REQUISITION_CD;
           begin
           open cursor_21;
           loop
           fetch cursor_21 into V_FILEID;
           exit when cursor_21%notfound; 
           if V_FILEID is not null then
             insert into well_del_temp_tbl(fileid) values(V_FILEID); 
           end if;
           end loop;
           end; 
           declare cursor cursor_22 is select FILEID  from PRO_LOG_RAPID_RESULTS  where REQUISITION_CD=V_REQUISITION_CD;
           begin
           open cursor_22;
           loop
           fetch cursor_22 into V_FILEID;
           exit when cursor_22%notfound; 
           if V_FILEID is not null then
              insert into well_del_temp_tbl(fileid) values(V_FILEID);
            end if;
           end loop;
           end;   
           declare cursor cursor_23 is select FILEID  from PRO_LOG_RAPID_ORIGINAL_DATA where JOB_PLAN_CD like V_REQUISITION_CD||'%'; 
           begin
           open cursor_23;
           loop
           fetch cursor_23 into V_FILEID;
           exit when cursor_23%notfound; 
           if V_FILEID is not null then
              insert into well_del_temp_tbl(fileid) values(V_FILEID); 
            end if;
           end loop;
           end;
           --删除计划任务书
            delete from DM_LOG_OPS_PLAN where REQUISITION_CD = V_REQUISITION_CD; 
           --删除流程数据
            delete from SYS_WORK_FLOW where OBJ_ID like V_REQUISITION_CD||'%';
        end loop;
        close cursor_2;
        end;     
        declare cursor cursor_3 is select PROCESS_ID from DM_LOG_PROCESS where DRILL_JOB_ID=V_DRILL_JOB_ID;
        begin
        open cursor_3;
        loop
        fetch cursor_3 into V_PROCESS_ID;
        exit when cursor_3%notfound;
           declare cursor cursor_31 is select FILEID  from PRO_LOG_PROCESSING_CURVE where PROCESS_ID=V_PROCESS_ID;
           begin
           open cursor_31;
           loop
           fetch cursor_31 into V_FILEID;
           exit when cursor_31%notfound; 
           if V_FILEID is not null then
              insert into well_del_temp_tbl(fileid) values(V_FILEID); 
            end if;
           end loop;
           end;
           
           declare cursor cursor_32 is select FILEID  from SYS_PROCESSING_UPLOADFILE where PROCESS_ID=V_PROCESS_ID;
           begin
           open cursor_32;
           loop
           fetch cursor_32 into V_FILEID;
           exit when cursor_32%notfound; 
           if V_FILEID  is not null then
              insert into well_del_temp_tbl(fileid) values(V_FILEID); 
            end if;
           end loop;
           end;
           delete from SYS_WORK_FLOW where OBJ_ID =V_PROCESS_ID;
        end loop;
        close cursor_3;
        end;
        --删除任务通知单
        delete from DM_LOG_TASK where DRILL_JOB_ID=V_DRILL_JOB_ID;
        --删除解释处理作业
        delete from DM_LOG_PROCESS where DRILL_JOB_ID=V_DRILL_JOB_ID;
    end loop;
    close cursor_1;
    end;

   --删除作业项目  
  delete from COM_JOB_INFO where WELL_ID=WellID;
  --删除井筒
  delete from COM_WELLBORE_BASIC where WELL_ID=WellID;
  --删除钻井专业作业井段
  delete from COM_JOB_INTERVAL where WELL_ID=WellID;
  --删除井身结构图
  delete from COM_WELLSTRUCTURE_DRAW where WELL_ID=WellID;
  --删除地层分层数据
  delete from DM_LOG_STRATA_LAYERED where WELL_ID=WellID;
  --删除井身结构数据
  delete from COM_WELLSTRUCTURE_DATA where WELL_ID=WellID;
  --删除井
  delete from COM_WELL_BASIC where WELL_ID=WellID;
  
  declare cursor cursor4 is select b.fileid,a.uploadid from sys_file_upload a,well_del_temp_tbl b where a.fileid=b.fileid;
   begin
   open cursor4;
   loop 
   fetch cursor4 into V_FILEID,V_UploadID;
       exit when cursor4%notfound;
        if V_UploadID is not null then
          update well_del_temp_tbl set uploadid=V_UploadID where fileid=V_FILEID;
        end if;
    end loop;
    close cursor4;
    end; 
    
  open p_cursor for select a.fileid,a.uploadid,b.sha1,b.md5,b.length,b.pathmain,b.pathid from well_del_temp_tbl a,sys_upload b where a.uploadid=b.uploadid;  

  commit;
  exception 
  when others then
  Raise;
  rollback;
END DEL_WELL_DATA;