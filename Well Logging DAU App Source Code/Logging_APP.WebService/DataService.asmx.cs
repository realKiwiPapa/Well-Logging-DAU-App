using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
//using System.Data.OracleClient;
using Oracle.DataAccess.Client;
using System.Web.Services.Protocols;
using Logging_App.Utility;
using Maticsoft.DBUtility;
using Logging_App.WebService.DAL;

namespace Logging_App.WebService
{
    /// <summary>
    /// DataService 的摘要说明
    /// </summary>
    [WebService(Namespace = "Logging_App.WebService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class DataService : System.Web.Services.WebService
    {

        #region
        [WebMethod(Description = "", EnableSession = true)]
        public DataSet SearchWellBasic(byte[] searchModel)
        {
            var model = Utility.ModelHelper.DeserializeObject(searchModel) as Model.COM_WELL_BASIC;
            var parameters = new List<OracleParameter>();
            var whereSql = new StringBuilder("");
            if (model != null)
            {
                if (!string.IsNullOrEmpty(model.WELL_LEGAL_NAME))
                {
                    whereSql.Append(" and regexp_like(WELL_LEGAL_NAME,:WELL_LEGAL_NAME,'i')");
                    parameters.Add(ServiceUtils.CreateOracleParameter(":WELL_LEGAL_NAME", OracleDbType.Varchar2, model.WELL_LEGAL_NAME));
                }
                if (!string.IsNullOrEmpty(model.WELL_STRUCT_UNIT_NAME))
                {
                    whereSql.Append(" and regexp_like(WELL_STRUCT_UNIT_NAME,:WELL_STRUCT_UNIT_NAME,'i')");
                    parameters.Add(ServiceUtils.CreateOracleParameter(":WELL_STRUCT_UNIT_NAME", OracleDbType.Varchar2, model.WELL_STRUCT_UNIT_NAME));
                }
                if (!string.IsNullOrEmpty(model.PART_UNITS))
                {
                    whereSql.Append(" and PART_UNITS=:PART_UNITS");
                    parameters.Add(ServiceUtils.CreateOracleParameter(":PART_UNITS", OracleDbType.Varchar2, model.PART_UNITS));
                }
            }
            return DbHelperOra.Query("select well_id,well_name,well_legal_name,beog_location,WELL_STRUCT_UNIT_NAME,STRUCTURAL_LOCATION,PART_UNITS from com_well_basic where 1=1 " + whereSql.ToString() + " order by cast(well_id as int) desc", parameters.ToArray());
        }

        [WebMethod(EnableSession = true)]
        public bool SaveWellBasic(byte[] byteModel)
        {
            var model = Utility.ModelHelper.DeserializeObject(byteModel) as Model.COM_WELL_BASIC;
            if (model == null) return false;
            StringBuilder strSql = new StringBuilder();
            var parameters = new List<OracleParameter>();

            if (string.IsNullOrWhiteSpace(model.WELL_ID))
            {
                strSql.Append("insert into COM_WELL_BASIC(");
                strSql.Append("WELL_ID,SITE_ID,WELL_NAME,WELL_LEGAL_NAME,WELL_UWI,WELL_STRUCT_UNIT_DESCRIPTION,WELL_FIELD_UNIT_NAME,WELL_STRUCT_UNIT_NAME,BEOG_LOCATION,STRUCTURAL_LOCATION,TRAVERSE_LINE_LOCATION,SURVEY_X_AXIS,SURVEY_Y_AXIS,GROUND_ELEVATION,SYSTEM_DATUM_OFFSET,RANGER_X_AXIS,RANGER_Y_AXIS,MAGNETIC_DECLINATION,CASING_TOP_DEPTH,WELL_HEAD_LONGITUDE,WELL_HEAD_LATITUDE,LOC_COUNTRY,LOC_STATE,P_LOC_CITY,LOC_COUNTY,PART_UNITS,WELL_HEAD_FLANGE,DRILL_GEO_DES_FILEID,DRILL_ENG_DES_FILEID,WELL_DRILL_CONDITION,BX_HEIGHT");
                strSql.Append(") values (");
                strSql.Append("WELL_ID_SEQUENCE.nextval,:SITE_ID,:WELL_NAME,:WELL_LEGAL_NAME,:WELL_UWI,:WELL_STRUCT_UNIT_DESCRIPTION,:WELL_FIELD_UNIT_NAME,:WELL_STRUCT_UNIT_NAME,:BEOG_LOCATION,:STRUCTURAL_LOCATION,:TRAVERSE_LINE_LOCATION,:SURVEY_X_AXIS,:SURVEY_Y_AXIS,:GROUND_ELEVATION,:SYSTEM_DATUM_OFFSET,:RANGER_X_AXIS,:RANGER_Y_AXIS,:MAGNETIC_DECLINATION,:CASING_TOP_DEPTH,:WELL_HEAD_LONGITUDE,:WELL_HEAD_LATITUDE,:LOC_COUNTRY,:LOC_STATE,:P_LOC_CITY,:LOC_COUNTY,:PART_UNITS,:WELL_HEAD_FLANGE,:DRILL_GEO_DES_FILEID,:DRILL_ENG_DES_FILEID,:WELL_DRILL_CONDITION,:BX_HEIGHT");
                strSql.Append(") ");

            }
            else
            {
                strSql.Append("update COM_WELL_BASIC set ");
                //strSql.Append(" WELL_ID = :WELL_ID , ");
                strSql.Append(" SITE_ID = :SITE_ID , ");
                strSql.Append(" WELL_NAME = :WELL_NAME , ");
                strSql.Append(" WELL_LEGAL_NAME = :WELL_LEGAL_NAME , ");
                strSql.Append(" WELL_UWI = :WELL_UWI , ");
                strSql.Append(" WELL_STRUCT_UNIT_DESCRIPTION = :WELL_STRUCT_UNIT_DESCRIPTION , ");
                strSql.Append(" WELL_FIELD_UNIT_NAME = :WELL_FIELD_UNIT_NAME , ");
                strSql.Append(" WELL_STRUCT_UNIT_NAME = :WELL_STRUCT_UNIT_NAME , ");
                strSql.Append(" BEOG_LOCATION = :BEOG_LOCATION , ");
                strSql.Append(" STRUCTURAL_LOCATION = :STRUCTURAL_LOCATION , ");
                strSql.Append(" TRAVERSE_LINE_LOCATION = :TRAVERSE_LINE_LOCATION , ");
                strSql.Append(" SURVEY_X_AXIS = :SURVEY_X_AXIS , ");
                strSql.Append(" SURVEY_Y_AXIS = :SURVEY_Y_AXIS , ");
                strSql.Append(" GROUND_ELEVATION = :GROUND_ELEVATION , ");
                strSql.Append(" SYSTEM_DATUM_OFFSET = :SYSTEM_DATUM_OFFSET , ");
                strSql.Append(" RANGER_X_AXIS = :RANGER_X_AXIS , ");
                strSql.Append(" RANGER_Y_AXIS = :RANGER_Y_AXIS , ");
                strSql.Append(" MAGNETIC_DECLINATION = :MAGNETIC_DECLINATION , ");
                strSql.Append(" CASING_TOP_DEPTH = :CASING_TOP_DEPTH , ");
                strSql.Append(" WELL_HEAD_LONGITUDE = :WELL_HEAD_LONGITUDE , ");
                strSql.Append(" WELL_HEAD_LATITUDE = :WELL_HEAD_LATITUDE , ");
                strSql.Append(" LOC_COUNTRY = :LOC_COUNTRY , ");
                strSql.Append(" LOC_STATE = :LOC_STATE , ");
                strSql.Append(" P_LOC_CITY = :P_LOC_CITY , ");
                strSql.Append(" LOC_COUNTY = :LOC_COUNTY , ");
                //strSql.Append(" DRILL_GEO_DES = :DRILL_GEO_DES , ");
                // strSql.Append(" DRILL_ENG_DES = :DRILL_ENG_DES , ");
                strSql.Append(" PART_UNITS = :PART_UNITS , ");
                strSql.Append(" WELL_HEAD_FLANGE = :WELL_HEAD_FLANGE , ");
                strSql.Append(" DRILL_GEO_DES_FILEID = :DRILL_GEO_DES_FILEID , ");
                strSql.Append(" DRILL_ENG_DES_FILEID = :DRILL_ENG_DES_FILEID,");
                strSql.Append(" WELL_DRILL_CONDITION=:WELL_DRILL_CONDITION,");
                strSql.Append(" BX_HEIGHT=:BX_HEIGHT");
                strSql.Append(" where WELL_ID=:WELL_ID  ");
                parameters.Add(ServiceUtils.CreateOracleParameter(":WELL_ID", OracleDbType.Char, model.WELL_ID));
            }
            parameters.AddRange(new OracleParameter[] {
                ServiceUtils.CreateOracleParameter(":SITE_ID", OracleDbType.Varchar2, model.SITE_ID) ,            
                ServiceUtils.CreateOracleParameter(":WELL_NAME", OracleDbType.Varchar2, model.WELL_NAME) ,            
                ServiceUtils.CreateOracleParameter(":WELL_LEGAL_NAME", OracleDbType.Varchar2, model.WELL_LEGAL_NAME) ,            
                ServiceUtils.CreateOracleParameter(":WELL_UWI", OracleDbType.Varchar2, model.WELL_UWI) ,            
                ServiceUtils.CreateOracleParameter(":WELL_STRUCT_UNIT_DESCRIPTION", OracleDbType.Varchar2, model.WELL_STRUCT_UNIT_DESCRIPTION) ,      
                ServiceUtils.CreateOracleParameter(":WELL_FIELD_UNIT_NAME", OracleDbType.Varchar2, model.WELL_FIELD_UNIT_NAME) ,            
                ServiceUtils.CreateOracleParameter(":WELL_STRUCT_UNIT_NAME", OracleDbType.Varchar2, model.WELL_STRUCT_UNIT_NAME) ,            
                ServiceUtils.CreateOracleParameter(":BEOG_LOCATION", OracleDbType.Varchar2, model.BEOG_LOCATION) ,            
                ServiceUtils.CreateOracleParameter(":STRUCTURAL_LOCATION", OracleDbType.Varchar2, model.STRUCTURAL_LOCATION) ,            
                ServiceUtils.CreateOracleParameter(":TRAVERSE_LINE_LOCATION", OracleDbType.Varchar2, model.TRAVERSE_LINE_LOCATION) ,            
                ServiceUtils.CreateOracleParameter(":SURVEY_X_AXIS", OracleDbType.Decimal, model.SURVEY_X_AXIS) ,            
                ServiceUtils.CreateOracleParameter(":SURVEY_Y_AXIS", OracleDbType.Decimal, model.SURVEY_Y_AXIS) ,            
                ServiceUtils.CreateOracleParameter(":GROUND_ELEVATION", OracleDbType.Decimal, model.GROUND_ELEVATION) ,            
                ServiceUtils.CreateOracleParameter(":SYSTEM_DATUM_OFFSET", OracleDbType.Decimal, model.SYSTEM_DATUM_OFFSET) ,            
                ServiceUtils.CreateOracleParameter(":RANGER_X_AXIS", OracleDbType.Decimal, model.RANGER_X_AXIS) ,            
                ServiceUtils.CreateOracleParameter(":RANGER_Y_AXIS", OracleDbType.Decimal, model.RANGER_Y_AXIS) ,            
                ServiceUtils.CreateOracleParameter(":MAGNETIC_DECLINATION", OracleDbType.Decimal, model.MAGNETIC_DECLINATION) ,            
                ServiceUtils.CreateOracleParameter(":CASING_TOP_DEPTH", OracleDbType.Decimal, model.CASING_TOP_DEPTH) ,            
                ServiceUtils.CreateOracleParameter(":WELL_HEAD_LONGITUDE", OracleDbType.Varchar2, model.WELL_HEAD_LONGITUDE) ,            
                ServiceUtils.CreateOracleParameter(":WELL_HEAD_LATITUDE", OracleDbType.Varchar2, model.WELL_HEAD_LATITUDE) ,            
                ServiceUtils.CreateOracleParameter(":LOC_COUNTRY", OracleDbType.Varchar2, model.LOC_COUNTRY) ,            
                ServiceUtils.CreateOracleParameter(":LOC_STATE", OracleDbType.Varchar2, model.LOC_STATE) ,            
                ServiceUtils.CreateOracleParameter(":P_LOC_CITY", OracleDbType.Varchar2, model.P_LOC_CITY) ,        
                ServiceUtils.CreateOracleParameter(":LOC_COUNTY", OracleDbType.Varchar2, model.LOC_COUNTY) ,
                //ServiceUtils.CreateOracleParameter(":DRILL_GEO_DES", OracleDbType.Blob, model.DRILL_GEO_DES) ,     
                //ServiceUtils.CreateOracleParameter(":DRILL_ENG_DES", OracleDbType.Blob, model.DRILL_ENG_DES) ,
                ServiceUtils.CreateOracleParameter(":PART_UNITS", OracleDbType.Varchar2, model.PART_UNITS) ,        
                ServiceUtils.CreateOracleParameter(":WELL_HEAD_FLANGE", OracleDbType.Char, model.WELL_HEAD_FLANGE) ,
                ServiceUtils.CreateOracleParameter(":DRILL_GEO_DES_FILEID", OracleDbType.Decimal, model.DRILL_GEO_DES_FILEID) ,     
                ServiceUtils.CreateOracleParameter(":DRILL_ENG_DES_FILEID", OracleDbType.Decimal, model.DRILL_ENG_DES_FILEID),
                ServiceUtils.CreateOracleParameter(":WELL_DRILL_CONDITION",OracleDbType.Varchar2,model.WELL_DRILL_CONDITION),
                ServiceUtils.CreateOracleParameter(":BX_HEIGHT",OracleDbType.Decimal,model.BX_HEIGHT)
            });
            if (DbHelperOra.ExecuteSql(strSql.ToString(), parameters.ToArray()) > 0) return true;
            return false;
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetWellBasic(string well_id)
        {
            return DbHelperOra.Query("select * from COM_WELL_BASIC where well_id=:WELL_ID", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":WELL_ID", OracleDbType.Char, well_id) });
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetWellboreList(string well_id)
        {
            return DbHelperOra.Query("select WELL_ID,WELLBORE_ID,WELLBORE_NAME,MAX_WELL_DEVIATION,MAX_WELL_DEVIATION_MD,DESIGN_VERTICAL_TVD,WELLBORE_PRODUCTION_DATE from com_wellbore_basic where well_id=:WELL_ID order by cast(wellbore_id as int) desc", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":WELL_ID", OracleDbType.Char, well_id) });
        }
        [WebMethod(EnableSession = true)]
        public DataSet GetWellbore(string wellbore_id)
        {
            return DbHelperOra.Query("select * from com_wellbore_basic where wellbore_id=:WELLBORE_ID", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":WELLBORE_ID", OracleDbType.Char, wellbore_id) });
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetWellStructure(string wellbore_id)
        {
            if (string.IsNullOrEmpty(wellbore_id)) return null;
            return DbHelperOra.Query("select * from COM_WELLSTRUCTURE_DATA where wellbore_id=:WELLBORE_ID", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":WELLBORE_ID", OracleDbType.Char, wellbore_id) });
        }

        [WebMethod(EnableSession = true)]
        public bool SaveWellbore(byte[] dataModel1, byte[] dataModel2)
        {
            var model1 = Utility.ModelHelper.DeserializeObject(dataModel1) as Model.COM_WELLBORE_BASIC;
            var model2 = Utility.ModelHelper.DeserializeObject(dataModel2) as Model.COM_WELLSTRUCTURE_DATA;
            if (model1 == null || model2 == null) return false;
            var strSql = new StringBuilder();
            var parameters = new List<OracleParameter>();
            strSql.Append("begin ");
            if (string.IsNullOrWhiteSpace(model1.WELLBORE_ID))
            {
                strSql.Append("insert into COM_WELLBORE_BASIC(");
                strSql.Append("WELL_ID,WELLBORE_ID,WELLBORE_WUI,P_WELLBORE_ID,WELLBORE_NAME,PURPOSE,MAX_WELL_DEVIATION,MAX_WELL_DEVIATION_MD,DESIGN_VERTICAL_TVD,WELLBORE_PRODUCTION_DATE,DEFLECTION_POINT_MD,MD,VERTICAL_WELL_TVD,PLUGBACK_MD,PLUGBACK_TVD,TRUE_PLUGBACKTOTAL_DEPTH,DESIGN_HORIZON,DESIGN_MD,ACTUAL_HORIZON,BTM_X_COORDINATE,BTM_Y_COORDINATE");
                strSql.Append(") values (");
                strSql.Append(":WELL_ID,WELLBORE_ID_SEQUENCE.nextval,:WELLBORE_WUI,:P_WELLBORE_ID,:WELLBORE_NAME,:PURPOSE,:MAX_WELL_DEVIATION,:MAX_WELL_DEVIATION_MD,:DESIGN_VERTICAL_TVD,:WELLBORE_PRODUCTION_DATE,:DEFLECTION_POINT_MD,:MD,:VERTICAL_WELL_TVD,:PLUGBACK_MD,:PLUGBACK_TVD,:TRUE_PLUGBACKTOTAL_DEPTH,:DESIGN_HORIZON,:DESIGN_MD,:ACTUAL_HORIZON,:BTM_X_COORDINATE,:BTM_Y_COORDINATE");
                strSql.Append(");");

                strSql.Append("insert into COM_WELLSTRUCTURE_DATA(");
                strSql.Append("WELLSTRUCTURE_ID,WELL_ID,WELLBORE_ID,NO,CASING_NAME,MD,HORIZON,BORE_SIZE,CASING_OD,BOINGTOOL_BOTTOM_DEPTH,SETTING_DEPTH,CHOKE_COIL_DEPTH,STAGE_COLLAR1_DEPTH,STAGE_COLLAR2_DEPTH,FIRST_CEMENT_TOP,SECOND_CEMENT_TOP,THIRD_CEMENT_TOP,CEMENT_METHOD,ARTIFICIAL_WELL_BOTTOM_DEPTH");
                strSql.Append(") values (");
                strSql.Append(":WELLSTRUCTURE_ID,:WELL_ID,WELLBORE_ID_SEQUENCE.currval,:NO,:CASING_NAME,:MD1,:HORIZON,:BORE_SIZE,:CASING_OD,:BOINGTOOL_BOTTOM_DEPTH,:SETTING_DEPTH,:CHOKE_COIL_DEPTH,:STAGE_COLLAR1_DEPTH,:STAGE_COLLAR2_DEPTH,:FIRST_CEMENT_TOP,:SECOND_CEMENT_TOP,:THIRD_CEMENT_TOP,:CEMENT_METHOD,:ARTIFICIAL_WELL_BOTTOM_DEPTH");
                strSql.Append(");");
            }
            else
            {
                strSql.Append("update COM_WELLBORE_BASIC set ");
                //strSql.Append(" WELL_ID = :WELL_ID , ");
                //strSql.Append(" WELLBORE_ID = :WELLBORE_ID , ");
                strSql.Append(" WELLBORE_WUI = :WELLBORE_WUI , ");
                strSql.Append(" P_WELLBORE_ID = :P_WELLBORE_ID , ");
                strSql.Append(" WELLBORE_NAME = :WELLBORE_NAME , ");
                strSql.Append(" PURPOSE = :PURPOSE , ");
                strSql.Append(" MAX_WELL_DEVIATION = :MAX_WELL_DEVIATION , ");
                strSql.Append(" MAX_WELL_DEVIATION_MD = :MAX_WELL_DEVIATION_MD , ");
                strSql.Append(" DESIGN_VERTICAL_TVD = :DESIGN_VERTICAL_TVD , ");
                strSql.Append(" WELLBORE_PRODUCTION_DATE = :WELLBORE_PRODUCTION_DATE , ");
                strSql.Append(" DEFLECTION_POINT_MD = :DEFLECTION_POINT_MD , ");
                strSql.Append(" MD = :MD , ");
                strSql.Append(" VERTICAL_WELL_TVD = :VERTICAL_WELL_TVD , ");
                strSql.Append(" PLUGBACK_MD = :PLUGBACK_MD , ");
                strSql.Append(" PLUGBACK_TVD = :PLUGBACK_TVD , ");
                strSql.Append(" TRUE_PLUGBACKTOTAL_DEPTH = :TRUE_PLUGBACKTOTAL_DEPTH , ");
                strSql.Append(" DESIGN_HORIZON = :DESIGN_HORIZON , ");
                strSql.Append(" DESIGN_MD = :DESIGN_MD , ");
                strSql.Append(" ACTUAL_HORIZON = :ACTUAL_HORIZON , ");
                strSql.Append(" BTM_X_COORDINATE = :BTM_X_COORDINATE , ");
                strSql.Append(" BTM_Y_COORDINATE = :BTM_Y_COORDINATE  ");
                strSql.Append(" where WELL_ID=:WELL_ID and WELLBORE_ID=:WELLBORE_ID;");

                strSql.Append("update COM_WELLSTRUCTURE_DATA set ");
                strSql.Append(" WELLSTRUCTURE_ID = :WELLSTRUCTURE_ID , ");
                //strSql.Append(" WELL_ID = :WELL_ID , ");
                //strSql.Append(" WELLBORE_ID = :WELLBORE_ID , ");
                strSql.Append(" NO = :NO , ");
                strSql.Append(" CASING_NAME = :CASING_NAME , ");
                strSql.Append(" MD = :MD1 , ");
                strSql.Append(" HORIZON = :HORIZON , ");
                strSql.Append(" BORE_SIZE = :BORE_SIZE , ");
                strSql.Append(" CASING_OD = :CASING_OD , ");
                strSql.Append(" BOINGTOOL_BOTTOM_DEPTH = :BOINGTOOL_BOTTOM_DEPTH , ");
                strSql.Append(" SETTING_DEPTH = :SETTING_DEPTH , ");
                strSql.Append(" CHOKE_COIL_DEPTH = :CHOKE_COIL_DEPTH , ");
                strSql.Append(" STAGE_COLLAR1_DEPTH = :STAGE_COLLAR1_DEPTH , ");
                strSql.Append(" STAGE_COLLAR2_DEPTH = :STAGE_COLLAR2_DEPTH , ");
                strSql.Append(" FIRST_CEMENT_TOP = :FIRST_CEMENT_TOP , ");
                strSql.Append(" SECOND_CEMENT_TOP = :SECOND_CEMENT_TOP , ");
                strSql.Append(" THIRD_CEMENT_TOP = :THIRD_CEMENT_TOP , ");
                strSql.Append(" CEMENT_METHOD = :CEMENT_METHOD , ");
                strSql.Append(" ARTIFICIAL_WELL_BOTTOM_DEPTH = :ARTIFICIAL_WELL_BOTTOM_DEPTH  ");
                strSql.Append(" where WELL_ID=:WELL_ID and WELLBORE_ID=:WELLBORE_ID;");

                parameters.Add(ServiceUtils.CreateOracleParameter(":WELLBORE_ID", OracleDbType.Char, model1.WELLBORE_ID));
            }
            parameters.AddRange(new OracleParameter[] {
			            ServiceUtils.CreateOracleParameter(":WELL_ID", OracleDbType.Char, model1.WELL_ID) ,            
                        ServiceUtils.CreateOracleParameter(":WELLBORE_WUI", OracleDbType.Varchar2, model1.WELLBORE_WUI) ,            
                        ServiceUtils.CreateOracleParameter(":P_WELLBORE_ID", OracleDbType.Char, model1.P_WELLBORE_ID) ,            
                        ServiceUtils.CreateOracleParameter(":WELLBORE_NAME", OracleDbType.Varchar2, model1.WELLBORE_NAME) ,            
                        ServiceUtils.CreateOracleParameter(":PURPOSE", OracleDbType.Varchar2, model1.PURPOSE) ,            
                        ServiceUtils.CreateOracleParameter(":MAX_WELL_DEVIATION", OracleDbType.Decimal, model1.MAX_WELL_DEVIATION) ,            
                        ServiceUtils.CreateOracleParameter(":MAX_WELL_DEVIATION_MD", OracleDbType.Decimal, model1.MAX_WELL_DEVIATION_MD) ,            
                        ServiceUtils.CreateOracleParameter(":DESIGN_VERTICAL_TVD", OracleDbType.Decimal, model1.DESIGN_VERTICAL_TVD) ,            
                        ServiceUtils.CreateOracleParameter(":WELLBORE_PRODUCTION_DATE", OracleDbType.Date, model1.WELLBORE_PRODUCTION_DATE) ,        
                        ServiceUtils.CreateOracleParameter(":DEFLECTION_POINT_MD", OracleDbType.Decimal, model1.DEFLECTION_POINT_MD) ,            
                        ServiceUtils.CreateOracleParameter(":MD", OracleDbType.Decimal, model1.MD) ,            
                        ServiceUtils.CreateOracleParameter(":VERTICAL_WELL_TVD", OracleDbType.Decimal, model1.VERTICAL_WELL_TVD) ,            
                        ServiceUtils.CreateOracleParameter(":PLUGBACK_MD", OracleDbType.Decimal, model1.PLUGBACK_MD) ,            
                        ServiceUtils.CreateOracleParameter(":PLUGBACK_TVD", OracleDbType.Decimal, model1.PLUGBACK_TVD) ,            
                        ServiceUtils.CreateOracleParameter(":TRUE_PLUGBACKTOTAL_DEPTH", OracleDbType.Decimal, model1.TRUE_PLUGBACKTOTAL_DEPTH) ,          
                        ServiceUtils.CreateOracleParameter(":DESIGN_HORIZON", OracleDbType.Varchar2, model1.DESIGN_HORIZON) ,            
                        ServiceUtils.CreateOracleParameter(":DESIGN_MD", OracleDbType.Decimal, model1.DESIGN_MD) ,            
                        ServiceUtils.CreateOracleParameter(":ACTUAL_HORIZON", OracleDbType.Varchar2, model1.ACTUAL_HORIZON) ,            
                        ServiceUtils.CreateOracleParameter(":BTM_X_COORDINATE", OracleDbType.Decimal, model1.BTM_X_COORDINATE) ,            
                        ServiceUtils.CreateOracleParameter(":BTM_Y_COORDINATE", OracleDbType.Decimal, model1.BTM_Y_COORDINATE),

                        ServiceUtils.CreateOracleParameter(":WELLSTRUCTURE_ID", OracleDbType.Varchar2, model2.WELLSTRUCTURE_ID) ,   
                        //ServiceUtils.CreateOracleParameter(":WELL_ID", OracleDbType.Char, model.WELL_ID) ,            
                        //ServiceUtils.CreateOracleParameter(":WELLBORE_ID", OracleDbType.Char, model.WELLBORE_ID) ,            
                        ServiceUtils.CreateOracleParameter(":NO", OracleDbType.Varchar2, model2.NO) ,            
                        ServiceUtils.CreateOracleParameter(":CASING_NAME", OracleDbType.Varchar2, model2.CASING_NAME) ,            
                        ServiceUtils.CreateOracleParameter(":MD1", OracleDbType.Decimal, model2.MD) ,            
                        ServiceUtils.CreateOracleParameter(":HORIZON", OracleDbType.Varchar2, model2.HORIZON) ,            
                        ServiceUtils.CreateOracleParameter(":BORE_SIZE", OracleDbType.Decimal, model2.BORE_SIZE) ,            
                        ServiceUtils.CreateOracleParameter(":CASING_OD", OracleDbType.Decimal, model2.CASING_OD) ,            
                        ServiceUtils.CreateOracleParameter(":BOINGTOOL_BOTTOM_DEPTH", OracleDbType.Decimal, model2.BOINGTOOL_BOTTOM_DEPTH) ,            
                        ServiceUtils.CreateOracleParameter(":SETTING_DEPTH", OracleDbType.Decimal, model2.SETTING_DEPTH) ,            
                        ServiceUtils.CreateOracleParameter(":CHOKE_COIL_DEPTH", OracleDbType.Decimal, model2.CHOKE_COIL_DEPTH) ,            
                        ServiceUtils.CreateOracleParameter(":STAGE_COLLAR1_DEPTH", OracleDbType.Decimal, model2.STAGE_COLLAR1_DEPTH) ,            
                        ServiceUtils.CreateOracleParameter(":STAGE_COLLAR2_DEPTH", OracleDbType.Decimal, model2.STAGE_COLLAR2_DEPTH) ,            
                        ServiceUtils.CreateOracleParameter(":FIRST_CEMENT_TOP", OracleDbType.Decimal, model2.FIRST_CEMENT_TOP) ,            
                        ServiceUtils.CreateOracleParameter(":SECOND_CEMENT_TOP", OracleDbType.Decimal, model2.SECOND_CEMENT_TOP) ,            
                        ServiceUtils.CreateOracleParameter(":THIRD_CEMENT_TOP", OracleDbType.Decimal, model2.THIRD_CEMENT_TOP) ,            
                        ServiceUtils.CreateOracleParameter(":CEMENT_METHOD", OracleDbType.Varchar2, model2.CEMENT_METHOD) ,            
                        ServiceUtils.CreateOracleParameter(":ARTIFICIAL_WELL_BOTTOM_DEPTH", OracleDbType.Decimal, model2.ARTIFICIAL_WELL_BOTTOM_DEPTH)      
            });
            strSql.Append(" end;");
            if (DbHelperOra.ExecuteSql(strSql.ToString(), parameters.ToArray()) > 0) return true;
            return false;
        }

        [WebMethod(EnableSession = true)]
        public bool SaveJobInfo(byte[] dataModel)
        {
            var model = Utility.ModelHelper.DeserializeObject(dataModel) as Model.COM_JOB_INFO;
            if (model == null) return false;
            var strSql = new StringBuilder();
            var parameters = new List<OracleParameter>();
            if (string.IsNullOrWhiteSpace(model.DRILL_JOB_ID))
            {
                strSql.Append("insert into COM_JOB_INFO(");
                strSql.Append("DRILL_JOB_ID,WELL_ID,WELL_ID_A7,WELL_ID_A1,WELL_JOB_NAME,ACTIVITY_NAME,FIELD,WELL_THE_MARKET,WELL_TYPE,WELL_SORT,TRUE_COMPLETION_FORMATION,COMPLETE_METHOD,JOB_PURPOSE,DRILL_UNIT,DESIGN_HORIZON,DESIGN_MD,WELLDONE_DEP,START_WELL_DATE,END_WELL_DATE,NOTE");
                strSql.Append(") values (");
                strSql.Append("DRILL_JOB_ID_SEQ.nextval,:WELL_ID,:WELL_ID_A7,:WELL_ID_A1,:WELL_JOB_NAME,:ACTIVITY_NAME,:FIELD,:WELL_THE_MARKET,:WELL_TYPE,:WELL_SORT,:TRUE_COMPLETION_FORMATION,:COMPLETE_METHOD,:JOB_PURPOSE,:DRILL_UNIT,:DESIGN_HORIZON,:DESIGN_MD,:WELLDONE_DEP,:START_WELL_DATE,:END_WELL_DATE,:NOTE");
                strSql.Append(") ");
            }
            else
            {
                strSql.Append("update COM_JOB_INFO set ");
                //strSql.Append(" DRILL_JOB_ID = :DRILL_JOB_ID , ");
                strSql.Append(" WELL_ID = :WELL_ID , ");
                strSql.Append(" WELL_ID_A7 = :WELL_ID_A7 , ");
                strSql.Append(" WELL_ID_A1 = :WELL_ID_A1 , ");
                strSql.Append(" WELL_JOB_NAME = :WELL_JOB_NAME , ");
                strSql.Append(" ACTIVITY_NAME = :ACTIVITY_NAME , ");
                strSql.Append(" FIELD = :FIELD , ");
                strSql.Append(" WELL_THE_MARKET = :WELL_THE_MARKET , ");
                strSql.Append(" WELL_TYPE = :WELL_TYPE , ");
                strSql.Append(" WELL_SORT = :WELL_SORT , ");
                strSql.Append(" TRUE_COMPLETION_FORMATION = :TRUE_COMPLETION_FORMATION , ");
                strSql.Append(" COMPLETE_METHOD = :COMPLETE_METHOD , ");
                strSql.Append(" JOB_PURPOSE = :JOB_PURPOSE , ");
                strSql.Append(" DRILL_UNIT = :DRILL_UNIT , ");
                strSql.Append(" DESIGN_HORIZON = :DESIGN_HORIZON , ");
                strSql.Append(" DESIGN_MD = :DESIGN_MD, ");
                strSql.Append(" WELLDONE_DEP = :WELLDONE_DEP, ");
                strSql.Append(" START_WELL_DATE = :START_WELL_DATE, ");
                strSql.Append(" END_WELL_DATE = :END_WELL_DATE, ");
                strSql.Append(" NOTE = :NOTE  ");
                strSql.Append(" where DRILL_JOB_ID=:DRILL_JOB_ID");

                parameters.Add(ServiceUtils.CreateOracleParameter(":DRILL_JOB_ID", OracleDbType.Varchar2, model.DRILL_JOB_ID));
            }
            parameters.AddRange(new OracleParameter[] {
                        ServiceUtils.CreateOracleParameter(":WELL_ID", OracleDbType.Char, model.WELL_ID) ,            
                        ServiceUtils.CreateOracleParameter(":WELL_ID_A7", OracleDbType.Varchar2, model.WELL_ID_A7) ,            
                        ServiceUtils.CreateOracleParameter(":WELL_ID_A1", OracleDbType.Varchar2, model.WELL_ID_A1) ,            
                        ServiceUtils.CreateOracleParameter(":WELL_JOB_NAME", OracleDbType.Varchar2, model.WELL_JOB_NAME) ,            
                        ServiceUtils.CreateOracleParameter(":ACTIVITY_NAME", OracleDbType.Varchar2, model.ACTIVITY_NAME) ,            
                        ServiceUtils.CreateOracleParameter(":FIELD", OracleDbType.Varchar2, model.FIELD) ,            
                        ServiceUtils.CreateOracleParameter(":WELL_THE_MARKET", OracleDbType.Varchar2, model.WELL_THE_MARKET) ,            
                        ServiceUtils.CreateOracleParameter(":WELL_TYPE", OracleDbType.Varchar2, model.WELL_TYPE) ,            
                        ServiceUtils.CreateOracleParameter(":WELL_SORT", OracleDbType.Varchar2, model.WELL_SORT) ,            
                        ServiceUtils.CreateOracleParameter(":TRUE_COMPLETION_FORMATION", OracleDbType.Varchar2, model.TRUE_COMPLETION_FORMATION) ,            
                        ServiceUtils.CreateOracleParameter(":COMPLETE_METHOD", OracleDbType.Varchar2, model.COMPLETE_METHOD) ,            
                        ServiceUtils.CreateOracleParameter(":JOB_PURPOSE", OracleDbType.Varchar2, model.JOB_PURPOSE) ,
			            ServiceUtils.CreateOracleParameter(":DRILL_UNIT", OracleDbType.Varchar2, model.DRILL_UNIT) ,            
                        ServiceUtils.CreateOracleParameter(":DESIGN_HORIZON", OracleDbType.Varchar2, model.DESIGN_HORIZON) ,            
                        ServiceUtils.CreateOracleParameter(":DESIGN_MD", OracleDbType.Decimal, model.DESIGN_MD),  
                        ServiceUtils.CreateOracleParameter(":WELLDONE_DEP", OracleDbType.Decimal, model.WELLDONE_DEP),
                        ServiceUtils.CreateOracleParameter(":START_WELL_DATE", OracleDbType.TimeStamp, model.START_WELL_DATE),
                        ServiceUtils.CreateOracleParameter(":END_WELL_DATE", OracleDbType.TimeStamp, model.END_WELL_DATE),
                        ServiceUtils.CreateOracleParameter(":NOTE", OracleDbType.Varchar2, model.NOTE)
            });
            if (DbHelperOra.ExecuteSql(strSql.ToString(), parameters.ToArray()) > 0) return true;
            return false;
        }

        [WebMethod(EnableSession = true)]
        public bool SaveDirllState(byte[] dataModel)
        {
            var model = Utility.ModelHelper.DeserializeObject(dataModel) as Model.DM_LOG_DRILL_STATE;
            if (model == null) return false;
            var strSql = new StringBuilder();
            var parameters = new List<OracleParameter>();
            if (string.IsNullOrWhiteSpace(model.DRILL_WELL_ID))
            {
                strSql.Append("insert into DM_LOG_DRILL_STATE(");
                strSql.Append("DRILL_WELL_ID,DRILL_JOB_ID,LAST_WEEK_MD,CURRENT_WEEK_MD,CURRENT_LAYER,CREATE_MAN,NOTE");//,CREATE_DATE
                strSql.Append(") values (");
                strSql.Append("DRILL_WELL_ID_SEQ.nextval,:DRILL_JOB_ID,:LAST_WEEK_MD,:CURRENT_WEEK_MD,:CURRENT_LAYER,:CREATE_MAN,:NOTE");//:CREATE_DATE
                strSql.Append(") ");
                parameters.Add(ServiceUtils.CreateOracleParameter(":DRILL_JOB_ID", OracleDbType.Varchar2, model.DRILL_JOB_ID));
            }
            else
            {
                strSql.Append("update DM_LOG_DRILL_STATE set ");
                //strSql.Append(" DRILL_WELL_ID = :DRILL_WELL_ID , ");
                //strSql.Append(" DRILL_JOB_ID = :DRILL_JOB_ID , ");
                strSql.Append(" LAST_WEEK_MD = :LAST_WEEK_MD , ");
                strSql.Append(" CURRENT_WEEK_MD = :CURRENT_WEEK_MD , ");
                strSql.Append(" CURRENT_LAYER = :CURRENT_LAYER , ");
                //strSql.Append(" CREATE_DATE = :CREATE_DATE , ");
                strSql.Append(" CREATE_MAN = :CREATE_MAN , ");
                strSql.Append(" NOTE = :NOTE  ");
                strSql.Append(" where DRILL_WELL_ID=:DRILL_WELL_ID  ");
                parameters.Add(ServiceUtils.CreateOracleParameter(":DRILL_WELL_ID", OracleDbType.Varchar2, model.DRILL_WELL_ID));
            }

            parameters.AddRange(new OracleParameter[] {          
                        ServiceUtils.CreateOracleParameter(":LAST_WEEK_MD", OracleDbType.Decimal, model.LAST_WEEK_MD) ,            
                        ServiceUtils.CreateOracleParameter(":CURRENT_WEEK_MD", OracleDbType.Decimal, model.CURRENT_WEEK_MD) ,            
                        ServiceUtils.CreateOracleParameter(":CURRENT_LAYER", OracleDbType.Char, model.CURRENT_LAYER) ,            
                        //ServiceUtils.CreateOracleParameter(":CREATE_DATE", OracleDbType.Date, model.CREATE_DATE) ,            
                        ServiceUtils.CreateOracleParameter(":CREATE_MAN", OracleDbType.Char, model.CREATE_MAN) ,            
                        ServiceUtils.CreateOracleParameter(":NOTE", OracleDbType.Varchar2, model.NOTE)             
            });
            if (DbHelperOra.ExecuteSql(strSql.ToString(), parameters.ToArray()) > 0) return true;
            return false;
        }
        [WebMethod(EnableSession = true)]
        public bool SaveGEO_DES_ITEM(byte[] dataModel)
        {
            var model = Utility.ModelHelper.DeserializeObject(dataModel) as Model.DM_LOG_GEO_DES_ITEM;
            if (model == null) return false;
            var strSql = new StringBuilder();
            var parameters = new List<OracleParameter>();
            if (string.IsNullOrWhiteSpace(model.DRILL_WELL_ID))
            {
                strSql.Append("insert into DM_LOG_GEO_DES_ITEM(");
                strSql.Append("DRILL_WELL_ID,DRILL_JOB_ID,LOG_MD,LOG_INTERVAL,LOG_LAYER,LOG_ITEM,LOG_SCALE,CREATE_DATE,CREATE_MAN,NOTE");
                strSql.Append(") values (");
                strSql.Append("DRILL_WELL_ID2_SEQ.nextval,:DRILL_JOB_ID,:LOG_MD,:LOG_INTERVAL,:LOG_LAYER,:LOG_ITEM,:LOG_SCALE,:CREATE_DATE,:CREATE_MAN,:NOTE");
                strSql.Append(") ");
                parameters.Add(ServiceUtils.CreateOracleParameter(":DRILL_JOB_ID", OracleDbType.Varchar2, model.DRILL_JOB_ID));
            }
            else
            {
                strSql.Append("update DM_LOG_GEO_DES_ITEM set ");
                //strSql.Append(" DRILL_WELL_ID = :DRILL_WELL_ID , ");
                //strSql.Append(" DRILL_JOB_ID = :DRILL_JOB_ID , ");
                strSql.Append(" LOG_MD = :LOG_MD , ");
                strSql.Append(" LOG_INTERVAL = :LOG_INTERVAL , ");
                strSql.Append(" LOG_LAYER = :LOG_LAYER , ");
                strSql.Append(" LOG_ITEM = :LOG_ITEM , ");
                strSql.Append(" LOG_SCALE = :LOG_SCALE , ");
                strSql.Append(" CREATE_DATE = :CREATE_DATE , ");
                strSql.Append(" CREATE_MAN = :CREATE_MAN , ");
                strSql.Append(" NOTE = :NOTE  ");
                strSql.Append(" where DRILL_WELL_ID=:DRILL_WELL_ID  ");
                parameters.Add(ServiceUtils.CreateOracleParameter(":DRILL_WELL_ID", OracleDbType.Varchar2, model.DRILL_WELL_ID));
            }
            parameters.AddRange(new OracleParameter[] {           
                        ServiceUtils.CreateOracleParameter(":LOG_MD", OracleDbType.Decimal, model.LOG_MD) ,            
                        ServiceUtils.CreateOracleParameter(":LOG_INTERVAL", OracleDbType.Char, model.LOG_INTERVAL) ,            
                        ServiceUtils.CreateOracleParameter(":LOG_LAYER", OracleDbType.Char, model.LOG_LAYER) ,            
                        ServiceUtils.CreateOracleParameter(":LOG_ITEM", OracleDbType.Varchar2, model.LOG_ITEM) ,            
                        ServiceUtils.CreateOracleParameter(":LOG_SCALE", OracleDbType.Char, model.LOG_SCALE) ,            
                        ServiceUtils.CreateOracleParameter(":CREATE_DATE", OracleDbType.Date, model.CREATE_DATE) ,            
                        ServiceUtils.CreateOracleParameter(":CREATE_MAN", OracleDbType.Char, model.CREATE_MAN) ,            
                        ServiceUtils.CreateOracleParameter(":NOTE", OracleDbType.Varchar2, model.NOTE)             
            });
            if (DbHelperOra.ExecuteSql(strSql.ToString(), parameters.ToArray()) > 0) return true;
            return false;
        }
        #endregion

        /// <summary>
        /// 获取通知单列表
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public DataSet GetTaskList(byte[] searchModel, int page, out int total)
        {
            int pageSize = 50;
            var model = Utility.ModelHelper.DeserializeObject(searchModel) as Model.DM_LOG_TASK;
            var parameters = new List<OracleParameter>();
            var SQL = new StringBuilder();
            var whereSql = new StringBuilder("");
            SQL.Append("SELECT A.Drill_Job_Id, ");
            SQL.Append("  B.Well_Job_Name, ");
            SQL.Append("  B1.WELL_LEGAL_NAME, ");
            SQL.Append("  A.Requisition_Type, ");
            SQL.Append("  A.Predicted_Logging_Items_Id, ");
            SQL.Append("  A.Requisition_Cd, ");
            SQL.Append("  A.Predicted_Logging_Date, ");
            SQL.Append("  A.Predicted_Logging_Interval, ");
            SQL.Append("  D.Log_Team_Id, ");
            SQL.Append("  D.Job_Chinese_Name, ");
            SQL.Append("  CASE ");
            SQL.Append("    WHEN Flow_Type = 1 ");
            SQL.Append("    THEN '指派到小队' ");
            SQL.Append("    WHEN Flow_Type = 4 ");
            SQL.Append("    THEN '小队施工' ");
            SQL.Append("    WHEN Flow_Type = 5 ");
            SQL.Append("    AND Flow_State = 2 ");
            SQL.Append("    THEN '归档完成' ");
            SQL.Append("    WHEN Flow_Type = 5 ");
            SQL.Append("    THEN '解释处理' ");
            SQL.Append("    ELSE '未建立任务书' ");
            SQL.Append("  END State ");
            SQL.Append("FROM Dm_Log_Task A, ");
            SQL.Append("  Com_Job_Info B, ");
            SQL.Append("  COM_WELL_BASIC B1, ");
            /*SQL.Append("  (SELECT Requisition_Cd, ");
            SQL.Append("    Flow_Type, ");
            SQL.Append("    Flow_State ");
            SQL.Append("  FROM ");
            SQL.Append("    (SELECT A.Requisition_Cd, ");
            SQL.Append("      A.Flow_Type, ");
            SQL.Append("      A.Flow_State, ");
            SQL.Append("      Row_Number() Over(Partition BY A.Requisition_Cd Order By ( ");
            SQL.Append("      CASE ");
            SQL.Append("        WHEN A.Flow_State=2 ");
            SQL.Append("        AND A.Flow_Type  =5 ");
            SQL.Append("        THEN 1 ");
            SQL.Append("        ELSE 0 ");
            SQL.Append("      END),A.Flow_Id DESC) Rn ");
            SQL.Append("    FROM ");
            SQL.Append("      (SELECT A.Requisition_Cd, ");
            SQL.Append("        C.Flow_Type, ");
            SQL.Append("        C.Flow_State, ");
            SQL.Append("        C.Obj_Id, ");
            SQL.Append("        C.Flow_Id, ");
            SQL.Append("        Row_Number() Over(Partition BY A.Job_Plan_Cd Order By C.Flow_Id DESC) Rn ");
            SQL.Append("      FROM Dm_Log_Ops_Plan A, ");
            SQL.Append("        Dm_Log_Source_Data B, ");
            SQL.Append("        Sys_Work_Flow_Now C ");
            SQL.Append("      WHERE (C.Flow_Type IN(1, 4) ");
            SQL.Append("      AND A.Job_Plan_Cd   = C.Obj_Id) ");
            SQL.Append("      OR (C.Flow_Type     = 5 ");
            SQL.Append("      AND B.Job_Plan_Cd   = A.Job_Plan_Cd ");
            SQL.Append("      AND B.Log_Data_Id   = C.Obj_Id) ");
            SQL.Append("      ) A ");
            SQL.Append("    WHERE Rn=1 ");
            SQL.Append("    ) ");
            SQL.Append("  WHERE Rn=1 ");
            SQL.Append("  ) C, ");*/
            SQL.Append(" VIEW_REQUISITION_STATE C, ");
            SQL.Append("  (SELECT Wm_Concat(DISTINCT B.Job_Chinese_Name) Job_Chinese_Name, ");
            SQL.Append("    Wm_Concat(A.Log_Team_Id) Log_Team_Id, ");
            SQL.Append("    A.Requisition_Cd ");
            SQL.Append("  FROM Dm_Log_Ops_Plan A, ");
            SQL.Append("    Pkl_Log_Ops_Perpose B ");
            SQL.Append("  WHERE A.Job_Id=B.Job_Id(+) ");

            if (model != null)
            {
                if (model.NOTE != null && model.NOTE.ToUpper() != "ALL")
                {
                    var arr = model.NOTE.Split('|');
                    var date = Convert.ToDateTime(DbHelperOra.GetSingle("select FN_QUERY_DATE(:F,:N) from dual",
                        ServiceUtils.CreateOracleParameter(":F", OracleDbType.Varchar2, arr[0]),
                        ServiceUtils.CreateOracleParameter(":N", OracleDbType.Decimal, int.Parse(arr[1]))));
                    if (int.Parse(arr[1]) == 0)
                    {
                        SQL.Append(" and a.REQUIREMENTS_TIME<:QUERY_DATE_1");
                        parameters.Add(ServiceUtils.CreateOracleParameter(":QUERY_DATE_1", OracleDbType.TimeStamp, date.AddDays(1)));
                    }
                    SQL.Append(" and a.REQUIREMENTS_TIME>=:QUERY_DATE");
                    parameters.Add(ServiceUtils.CreateOracleParameter(":QUERY_DATE", OracleDbType.TimeStamp, date));
                }
            }

            SQL.Append("  GROUP BY A.Requisition_Cd ");
            SQL.Append("  ) D ");
            SQL.Append("WHERE A.Drill_Job_Id = B.Drill_Job_Id ");
            SQL.Append("AND B.WELL_ID= B1.WELL_ID ");
            SQL.Append("AND A.Requisition_Cd = C.Requisition_Cd(+) ");
            SQL.Append("AND A.Requisition_Cd = D.Requisition_Cd(+) ");

            if (model != null)
            {
                if (!string.IsNullOrEmpty(model.WELL_JOB_NAME))
                {
                    whereSql.Append(" and b.WELL_JOB_NAME like :WELL_JOB_NAME");
                    parameters.Add(ServiceUtils.CreateOracleParameter(":WELL_JOB_NAME", OracleDbType.Varchar2, "%" + model.WELL_JOB_NAME + "%"));
                }
                if (model.REQUISITION_TYPE != null)
                {
                    whereSql.Append(" and regexp_like(REQUISITION_TYPE,:REQUISITION_TYPE,'i')");
                    parameters.Add(ServiceUtils.CreateOracleParameter(":REQUISITION_TYPE", OracleDbType.Varchar2, model.REQUISITION_TYPE));
                }
                if (model.DEPARTMENT_REQUISITION != null)
                {
                    whereSql.Append(" and b.DEPARTMENT_REQUISITION like '%'||:DEPARTMENT_REQUISITION||'%'");
                    parameters.Add(ServiceUtils.CreateOracleParameter(":DEPARTMENT_REQUISITION", OracleDbType.Decimal, model.DEPARTMENT_REQUISITION));
                }

                if (model.NOTE != null && model.NOTE.ToUpper() != "ALL")
                {
                    var arr = model.NOTE.Split('|');
                    var date = Convert.ToDateTime(DbHelperOra.GetSingle("select FN_QUERY_DATE(:F,:N) from dual",
                        ServiceUtils.CreateOracleParameter(":F", OracleDbType.Varchar2, arr[0]),
                        ServiceUtils.CreateOracleParameter(":N", OracleDbType.Decimal, int.Parse(arr[1]))));
                    if (int.Parse(arr[1]) == 0)
                    {
                        whereSql.Append(" and a.PREDICTED_LOGGING_DATE<:QUERY_DATE_1");
                        parameters.Add(ServiceUtils.CreateOracleParameter(":QUERY_DATE_1", OracleDbType.TimeStamp, date.AddDays(1)));
                    }
                    whereSql.Append(" and a.PREDICTED_LOGGING_DATE>=:QUERY_DATE");
                    parameters.Add(ServiceUtils.CreateOracleParameter(":QUERY_DATE", OracleDbType.TimeStamp, date));
                }
            }
            var finalSql = SQL.ToString() + whereSql.ToString() + " order by a.PREDICTED_LOGGING_DATE DESC Nulls Last,a.PREDICTED_LOGGING_ITEMS_ID DESC";
            total = Convert.ToInt32(DbHelperOra.GetSingle("select count(*) from(" + finalSql + ")", parameters.ToArray()));
            var pageSql = "select * from(select ROWNUM RN,K.* from(" + finalSql + ") K where ROWNUM<=" + page * pageSize + ") where RN>=" + ((page - 1) * pageSize + 1);
            return DbHelperOra.Query(pageSql, parameters.ToArray());
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetJobInfoList(byte[] searchModel)
        {
            var model = Utility.ModelHelper.DeserializeObject(searchModel) as Model.JobInfoSearch;
            var parameters = new List<OracleParameter>();
            var whereSql = new StringBuilder("");
            if (model != null)
            {
                if (!string.IsNullOrEmpty(model.WELL_JOB_NAME))
                {
                    whereSql.Append(" and regexp_like(A.WELL_JOB_NAME,:WELL_JOB_NAME,'i')");
                    parameters.Add(ServiceUtils.CreateOracleParameter(":WELL_JOB_NAME", OracleDbType.Varchar2, model.WELL_JOB_NAME));
                }
                if (!string.IsNullOrEmpty(model.ACTIVITY_NAME))
                {
                    whereSql.Append(" and regexp_like(A.ACTIVITY_NAME,:ACTIVITY_NAME,'i')");
                    parameters.Add(ServiceUtils.CreateOracleParameter(":ACTIVITY_NAME", OracleDbType.Varchar2, model.ACTIVITY_NAME));
                }
                if (!string.IsNullOrEmpty(model.WELL_TYPE))
                {
                    whereSql.Append(" and regexp_like(A.WELL_TYPE,:WELL_TYPE,'i')");
                    parameters.Add(ServiceUtils.CreateOracleParameter(":WELL_TYPE", OracleDbType.Varchar2, model.WELL_TYPE));
                }
                if (!string.IsNullOrEmpty(model.PART_UNITS))
                {
                    whereSql.Append(" and regexp_like(B.Part_Units,:PART_UNITS,'i')");
                    parameters.Add(ServiceUtils.CreateOracleParameter(":PART_UNITS", OracleDbType.Varchar2, model.PART_UNITS));
                }
                if (model.DRILL_STATE > -1)
                {
                    whereSql.Append(" and C.Create_Date>=add_months(sysdate, " + (model.DRILL_STATE + 1) * -1 + ")");
                }
            }
            return DbHelperOra.Query("Select A.Drill_Job_Id,A.Well_Id,A.Well_Job_Name,A.Activity_Name,A.Well_Type,A.Well_Sort,B.Part_Units,C.Create_Date From Com_Job_Info A,Com_Well_Basic B,(Select Drill_Job_Id,Max(Create_Date) As Create_Date From Dm_Log_Drill_State Where Create_Date Is Not Null Group By Drill_Job_Id) C Where A.Well_Id=B.Well_Id And A.Drill_Job_Id=C.Drill_Job_Id(+) " + whereSql.ToString() + " Order By cast(A.DRILL_JOB_ID as int) DESC,C.Create_Date Desc Nulls Last,A.Rowid DESC", parameters.ToArray());
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetJobInfo(string job_id)
        {
            return DbHelperOra.Query("select * FROM COM_JOB_INFO where DRILL_JOB_ID=:DRILL_JOB_ID",
                ServiceUtils.CreateOracleParameter(":DRILL_JOB_ID", OracleDbType.Varchar2, job_id));
        }
        [WebMethod(EnableSession = true)]
        public DataSet GetDrillState(string drill_well_id)
        {
            return DbHelperOra.Query("select * FROM DM_LOG_DRILL_STATE where DRILL_WELL_ID=:DRILL_WELL_ID",
                ServiceUtils.CreateOracleParameter(":DRILL_WELL_ID", OracleDbType.Varchar2, drill_well_id));
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetGEO_DES_Item(string drill_well_id)
        {
            return DbHelperOra.Query("select * FROM DM_LOG_GEO_DES_ITEM where DRILL_WELL_ID=:DRILL_WELL_ID",
    ServiceUtils.CreateOracleParameter(":DRILL_WELL_ID", OracleDbType.Varchar2, drill_well_id));
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetDrillStateList(string job_id)
        {
            return DbHelperOra.Query("Select A.Drill_Well_Id,A.Drill_Job_Id,A.Current_Week_Md,A.Current_Layer,A.Create_Date,A.Note,B.Drill_Unit,B.Design_Horizon,B.Design_Md,Lag(A.Current_Week_Md,1) Over(Order By to_Number(A.Drill_Well_Id)) As Last_Week_Md From Dm_Log_Drill_State A, Com_Job_Info B Where A.Drill_Job_Id=:DRILL_JOB_ID And A.Drill_Job_Id  =B.Drill_Job_Id",
                ServiceUtils.CreateOracleParameter(":DRILL_JOB_ID", OracleDbType.Varchar2, job_id));
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetGEO_DES_ItemList(string job_id)
        {
            return DbHelperOra.Query("select * FROM DM_LOG_GEO_DES_ITEM where DRILL_JOB_ID=:DRILL_JOB_ID",
    ServiceUtils.CreateOracleParameter(":DRILL_JOB_ID", OracleDbType.Varchar2, job_id));
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetWellComboBoxList()
        {
            return DbHelperOra.Query("select well_id,well_name from COM_WELL_BASIC order by well_name");
        }
        [WebMethod(EnableSession = true)]
        public DataSet GetWellType()
        {
            return DbHelperOra.Query("SELECT WELL_TYPE FROM PKL_LOG_WELL_TYPE ORDER BY WELL_TYPE_ID");
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetWellSort()
        {
            return DbHelperOra.Query("SELECT WELL_SORT FROM PKL_LOG_WELL_SORT ORDER BY WELL_SORT_ID");
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetFileTemplate()
        {
            return DbHelperOra.Query("select filename,filefromat,filedata,relativeview from pkl_log_filetemplate order by id");
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetA1DataByView(string view_name, byte[] data)
        {
            var pList = ModelHelper.DeserializeObject(data) as List<string>;
            DataSet ds = new DataSet();
            foreach (var pid in pList)
            {
                //var t_ds = DbHelperOra.Query("select * from " + view_name + " where process_id=:process_id",
                // ServiceUtils.CreateOracleParameter(":process_id", OracleDbType.Varchar2, pid));

                var t_ds = DbHelperOra.Query("select rownum 序号,a.* from (select * from " + view_name + ") a where a.process_id=:process_id",
                 ServiceUtils.CreateOracleParameter(":process_id", OracleDbType.Varchar2, pid));
                ds.Merge(t_ds);
            }
            return ds;
        }

        [WebMethod(EnableSession = true)]
        public bool PushFileToA1(byte[] bytes)
        {
            bool result = true;
            var fileDic = ModelHelper.DeserializeObject(bytes) as Dictionary<string, string>;
            var ftpHelper = new FtpHelper("A1");
            var path = FileUpload.PathList[0].name;
            path = path.Remove(path.LastIndexOf('\\') + 1);
            //var dir=DateTime.Now.ToString("yyyyMM");
            try
            {
                //if (!ftpHelper.DirectoryExist(dir))
                //    ftpHelper.MakeDir(dir);
                foreach (var name in fileDic.Keys)
                {
                    var filePath = path + fileDic[name];
                    if (!File.Exists(filePath)) continue;
                    if (!ftpHelper.FileExist(null, name))
                        ftpHelper.Upload(null, name, filePath);
                }
            }
            catch (Exception ex)
            {
                result = false;
                File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "LogFile\\a1_err.txt", DateTime.Now + "--错误:" + ex.Message + "\r\n");
            }
            return result;
        }

        /// <summary>
        /// 获取计划任务书列表
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public DataSet GetPlanList(byte[] searchModel, int page, out int total)
        {
            int pageSize = 50;
            var model = Utility.ModelHelper.DeserializeObject(searchModel) as Model.LogPlanSearch;
            var parameters = new List<OracleParameter>();
            var SQL = new StringBuilder();
            var whereSql = new StringBuilder("");
            SQL.Append("SELECT A.Requisition_Cd, ");
            SQL.Append("  A.Job_Plan_Cd, ");
            SQL.Append("  A.Received_Inform_Time, ");
            SQL.Append("  A.Requirements_Time, ");
            SQL.Append("  A.Log_Type, ");
            SQL.Append("  A.Prelogging_Interval, ");
            SQL.Append("  A.Log_Team_Id, ");
            SQL.Append("  A.Job_Id, ");
            SQL.Append("  B.Predicted_Logging_Items_Id, ");
            SQL.Append("  C.Well_Job_Name, ");
            SQL.Append("  C1.WELL_LEGAL_NAME ");
            SQL.Append("FROM Dm_Log_Ops_Plan A, ");
            SQL.Append("  Dm_Log_Task B, ");
            SQL.Append("  Com_Job_Info C, ");
            SQL.Append("  COM_WELL_BASIC C1 "); ;
            SQL.Append("WHERE A.Requisition_Cd=B.Requisition_Cd ");
            SQL.Append("AND C.WELL_ID= C1.WELL_ID ");
            SQL.Append("AND B.Drill_Job_Id    =C.Drill_Job_Id");
            if (model != null)
            {
                if (!string.IsNullOrEmpty(model.JOB_PLAN_CD))
                {
                    whereSql.Append(" and ((UPPER(A.Log_Team_Id) like :JOB_PLAN_CD ) or (UPPER(C.Well_Job_Name) like :JOB_PLAN_CD ))");
                    parameters.Add(ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, "%" + model.JOB_PLAN_CD.ToUpper() + "%"));
                }

                if (model.NOTE != null && model.NOTE.ToUpper() != "ALL")
                {
                    var arr = model.NOTE.Split('|');
                    var date = Convert.ToDateTime(DbHelperOra.GetSingle("select FN_QUERY_DATE(:F,:N) from dual",
                        ServiceUtils.CreateOracleParameter(":F", OracleDbType.Varchar2, arr[0]),
                        ServiceUtils.CreateOracleParameter(":N", OracleDbType.Decimal, int.Parse(arr[1]))));
                    if (int.Parse(arr[1]) == 0)
                    {
                        whereSql.Append(" and A.Requirements_Time<:QUERY_DATE_1");
                        parameters.Add(ServiceUtils.CreateOracleParameter(":QUERY_DATE_1", OracleDbType.TimeStamp, date.AddDays(1)));
                    }
                    whereSql.Append(" and A.Requirements_Time>=:QUERY_DATE");
                    parameters.Add(ServiceUtils.CreateOracleParameter(":QUERY_DATE", OracleDbType.TimeStamp, date));
                }
            }
            var finalSql = SQL.ToString() + whereSql.ToString() + " order by A.Requirements_Time DESC Nulls Last,B.PREDICTED_LOGGING_ITEMS_ID DESC";
            total = Convert.ToInt32(DbHelperOra.GetSingle("select count(*) from(" + finalSql + ")", parameters.ToArray()));
            var pageSql = "select * from(select ROWNUM RN,K.* from(" + finalSql + ") K where ROWNUM<=" + page * pageSize + ") where RN>=" + ((page - 1) * pageSize + 1);
            return DbHelperOra.Query(pageSql, parameters.ToArray());
        }

        /// <summary>
        /// 获取计划任务书数据
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public DataSet GetPlanData(string JOB_PLAN_CD)
        {
            return DbHelperOra.Query("select * from dm_log_ops_plan where job_plan_cd=:JOB_PLAN_CD",
                new OracleParameter[]{
                    ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD",OracleDbType.Varchar2,JOB_PLAN_CD)
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public DataSet GetWellJobNameList()
        {
            return DbHelperOra.Query("SELECT DRILL_JOB_ID,well_job_name FROM com_job_info ORDER BY well_job_name");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public DataSet GetRequisitionCdList()
        {
            StringBuilder SQL = new StringBuilder();
            SQL.Append("SELECT A.REQUISITION_CD, ");
            SQL.Append("  A.PREDICTED_LOGGING_ITEMS_ID ");
            SQL.Append("FROM DM_LOG_TASK A, ");
            SQL.Append("  SYS_WORK_FLOW_NOW B ");
            SQL.Append("WHERE A.REQUISITION_CD     =B.OBJ_ID ");
            SQL.Append("AND B.TARGET_LOGINNAME     =:LOGINNAME ");
            SQL.Append("AND B.FLOW_TYPE            =0 ");
            SQL.Append("AND B.FLOW_STATE           =0 ");
            SQL.Append("AND ( B.TARGET_LOGINNAME   =B.SOURCE_LOGINNAME ");
            SQL.Append("OR B.TARGET_LOGINNAME NOT IN ");
            SQL.Append("  (SELECT COL_LOGINNAME FROM HS_ROLE WHERE COL_ROLETYPE=0 ");
            SQL.Append("  ) ) ");
            SQL.Append("ORDER BY REQUISITION_CD");
            return DbHelperOra.Query(SQL.ToString(),
                new OracleParameter[]{
                    ServiceUtils.CreateOracleParameter(":LOGINNAME",OracleDbType.Varchar2,ServiceUtils.GetUserInfo().COL_LOGINNAME)
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wellJobName"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public DataSet GetWellBaseInfo(string wellJobName)
        {
            if (string.IsNullOrWhiteSpace(wellJobName)) return null;
            return DbHelperOra.Query("SELECT a.* FROM com_well_basic a, com_job_info b where b.well_job_name=:welljobname and a.well_id=b.well_id", new OracleParameter[] { new OracleParameter(":welljobname", wellJobName) });
        }
        [WebMethod(EnableSession = true)]
        public DataSet GetComJobInfo(string wellJobName)
        {
            if (string.IsNullOrWhiteSpace(wellJobName)) return null;
            return DbHelperOra.Query("SELECT * FROM  com_job_info where well_job_name=:welljobname", new OracleParameter[] { new OracleParameter(":welljobname", wellJobName) });
        }
        /// <summary>
        /// 获取预测项目列表
        /// </summary>
        /// <param name="itemID"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public DataSet GetPredictedItems(string itemID)
        {
            if (string.IsNullOrWhiteSpace(itemID)) return null;
            return DbHelperOra.Query("select a.PREDICTED_LOGGING_ITEMS_ID,a.note , b.LOG_ITEM_ID,b.LOG_ITEM_TYPE as PREDICTED_LOGGING_NAME,a.pre_st_dep,a.pre_en_dep,a.pre_scale FROM dm_log_predicted_item a,PKL_LOG_ITEM b WHERE b.LOG_ITEM_ID=a.LOG_ITEM_ID and predicted_logging_items_id=:itemid", new OracleParameter[] { new OracleParameter(":itemid", itemID) });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public DataSet GetJobIDlist()
        {
            return DbHelperOra.Query("select job_id,job_chinese_name from pkl_log_ops_perpose");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public DataSet GetlogItems()
        {
            return DbHelperOra.Query("SELECT log_item_id,log_item_type,parent_id FROM pkl_log_item");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public DataSet GetComboxList_QueryDate()
        {
            return DbHelperOra.Query("SELECT QUREY_DATE,DATE_VALUE FROM PKL_LOG_QUERY_DATE ORDER BY DATE_ITEM_ID");
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetRequisitionTypes()
        {
            return DbHelperOra.Query("SELECT REQUISITION_TYPE_NAME,REQUISITION_TYPE_ID FROM PKL_LOG_REQUISITION_TYPE ORDER BY REQUISITION_TYPE_ID");
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetComboBoxList_作业目的()
        {
            return DbHelperOra.Query("SELECT JOB_ID,JOB_CHINESE_NAME FROM PKL_LOG_OPS_PERPOSE order by job_id");
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetComboBoxList_市场类型()
        {
            return DbHelperOra.Query("SELECT MARKET_TYPE_ID,MARKET_TYPE FROM PKL_LOG_MARKET_TYPE order by MARKET_TYPE_ID");
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetComboBoxList_甲方来电单位()
        {
            return DbHelperOra.Query("SELECT DEPARTMENT_REQUISITION_ID,DEPARTMENT_REQUISITION_NAME FROM PKL_LOG_DEPARTMENT_REQUISITION order by DEPARTMENT_REQUISITION_ID");
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetComboBoxList_执行单位()
        {
            return DbHelperOra.Query("SELECT EXEC_TASK_UNIT_ID,EXEC_TASK_UNIT_NAME FROM PKL_LOG_EXEC_UNIT order by EXEC_TASK_UNIT_ID");
        }
        [WebMethod(EnableSession = true)]
        public DataSet GetComboBoxList_COMBINATION_NAME()
        {
            return DbHelperOra.Query("SELECT COMBINATION_NAME_NAME FROM PKL_LOG_WORK_TYPE order by combination_name_id");
        }


        [WebMethod(EnableSession = true)]
        public DataSet GetComboBoxList_下井趟次号(string JOB_PLAN_CD)
        {
            if (string.IsNullOrWhiteSpace(JOB_PLAN_CD)) return null;
            DataSet dt = DbHelperOra.Query("select down_well_sequence as value,to_char(down_well_sequence) as name  from dm_log_work_details where job_plan_cd=:JOB_PLAN_CD order by down_well_sequence",
                new OracleParameter[]{
                    ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, JOB_PLAN_CD)});
            DataRow dr = dt.Tables[0].NewRow();
            dr[0] = 0;
            dr[1] = "未下井";
            dt.Tables[0].Rows.InsertAt(dr, 0);
            return dt;
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetComboBoxList_CurveQualityTypes()
        {
            return DbHelperOra.Query("SELECT CURVE_QUALITY_TYPE FROM PKL_LOG_CURVE_QUALITY ORDER BY CURVE_QUALITY_ID");
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetComboBoxList_InterpretCenter()
        {
            return DbHelperOra.Query("SELECT LOG_INTERPRETCENTER_NAME FROM PKL_LOG_INTERPRETCENTER");
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetComboBoxList_LogSeries()
        {
            return DbHelperOra.Query("SELECT LOG_SERIES_NAME FROM PKL_LOG_LOGSERIES");
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetComboBoxList_DataFormat()
        {
            return DbHelperOra.Query("SELECT LOG_DATA_FORMA_CODE FROM PKL_LOG_DATA_FORMAT");
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetComboBoxList_AcceptanceWay()
        {
            return DbHelperOra.Query("SELECT ACCEPTANCE_WAY_NAME FROM PKL_LOG_ACCEPTANCE_WAY");
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetComboBoxList_ProcessSoftware()
        {
            return DbHelperOra.Query("SELECT LOG_PROCESSSOFTWARE_NAME FROM PKL_LOG_PROCESSSOFTWARE");
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetComboBoxList_SlopProperties()
        {
            return DbHelperOra.Query("SELECT LOG_SLOPPROPERTIES_NAME FROM PKL_LOG_SLOPPROPERTIES");
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetComboBoxList_ArchiveItemCodes()
        {
            return DbHelperOra.Query("SELECT MIN(item_index) AS item_index,ITEM_NAME,','||wm_concat(ARCHIVE_NAME)||',' AS ARCHIVE_NAME FROM PKL_LOG_ITEM_ARCHIVE GROUP BY item_name ORDER BY item_index");
        }
        //获取静态流程链列表
        [WebMethod(EnableSession = true)]
        public DataSet GetStaticWorkflowList(byte[] _searchModel)
        {
            var model = Utility.ModelHelper.DeserializeObject(_searchModel) as Model.SYS_STATIC_WORK_FLOW;
            var parameters = new List<OracleParameter>();
            var whereSql = new StringBuilder();
            if (model != null)
            {
                if (!string.IsNullOrEmpty(model.FLOW_NAME))
                {
                    whereSql.Append(" and regexp_like(FLOW_NAME,:FLOW_NAME,'i')");
                    parameters.Add(ServiceUtils.CreateOracleParameter(":FLOW_NAME", OracleDbType.Varchar2, model.FLOW_NAME));
                }
                if (!string.IsNullOrEmpty(model.FLOW_TYPE))
                {
                    whereSql.Append(" and regexp_like(FLOW_TYPE,:FLOW_TYPE,'i')");
                    parameters.Add(ServiceUtils.CreateOracleParameter(":FLOW_TYPE", OracleDbType.Varchar2, model.FLOW_TYPE));
                }
                if (!string.IsNullOrEmpty(model.CREATE_NAME))
                {
                    whereSql.Append(" and regexp_like(CREATE_NAME,:CREATE_NAME,'i')");
                    parameters.Add(ServiceUtils.CreateOracleParameter(":CREATE_NAME", OracleDbType.Varchar2, model.CREATE_NAME));
                }
                return DbHelperOra.Query("select * from SYS_STATIC_WORK_FLOW where 1=1" + whereSql.ToString(), parameters.ToArray());
            }
            return null;
        }
        //通过流程ID获取静态流程链
        [WebMethod(EnableSession = true)]
        public DataSet GetStaticWorkflow(string flow_id)
        {
            return DbHelperOra.Query("select * FROM SYS_STATIC_WORK_FLOW where FLOW_ID=:FLOWID",
                ServiceUtils.CreateOracleParameter(":FLOWID", OracleDbType.Varchar2, flow_id));
        }

        //保存静态流程链
        [WebMethod(EnableSession = true)]
        public bool Save_Static_Work_Flow(byte[] _model)
        {
            if (_model == null) return false;
            var model = Utility.ModelHelper.DeserializeObject(_model) as Model.SYS_STATIC_WORK_FLOW;
            List<OracleParameter> parameters = new List<OracleParameter>();
            var strsql = new StringBuilder();
            if (model.FLOW_ID == 0)
            {
                strsql.Append("insert into SYS_STATIC_WORK_FLOW(");
                strsql.Append("FLOW_ID,FLOW_NAME,FLOW_NODE_NUM,INPUTER,INPUTER_ID,FLOW_TYPE,CREATE_NAME,NOTE");
                strsql.Append(") values (");
                strsql.Append("STATIC_FLOWID_SEQ.NEXTVAL,:FLOW_NAME,:FLOW_NODE_NUM,:INPUTER,:INPUTER_ID,:FLOW_TYPE,:CREATE_NAME,:NOTE");
                strsql.Append(")");
            }
            else
            {
                strsql.Append("update SYS_STATIC_WORK_FLOW set ");
                strsql.Append("FLOW_NAME=:FLOW_NAME,");
                strsql.Append("FLOW_NODE_NUM=:FLOW_NODE_NUM,");
                strsql.Append("INPUTER=:INPUTER,");
                strsql.Append("INPUTER_ID=:INPUTER_ID,");
                strsql.Append("FLOW_TYPE=:FLOW_TYPE,");
                strsql.Append("CREATE_NAME=:CREATE_NAME,");
                strsql.Append("NOTE=:NOTE");
                strsql.Append(" where FLOW_ID=:FLOW_ID");
                parameters.Add(ServiceUtils.CreateOracleParameter(":FLOW_ID", OracleDbType.Decimal, model.FLOW_ID));
            }

            parameters.AddRange(new OracleParameter[]{
                ServiceUtils.CreateOracleParameter(":FLOW_NAME",OracleDbType.Varchar2,model.FLOW_NAME),
                ServiceUtils.CreateOracleParameter(":FLOW_NODE_NUM",OracleDbType.Decimal,model.FLOW_NODE_NUM),
                //ServiceUtils.CreateOracleParameter(":TARGET_LOGINNAME",OracleDbType.Varchar2,model.TARGET_NAME),
                ServiceUtils.CreateOracleParameter(":INPUTER",OracleDbType.Varchar2,model.INPUTER),
                ServiceUtils.CreateOracleParameter(":INPUTER_ID",OracleDbType.Varchar2,model.INPUTER_ID),
                ServiceUtils.CreateOracleParameter(":FLOW_TYPE",OracleDbType.Varchar2,model.FLOW_TYPE),
                ServiceUtils.CreateOracleParameter(":CREATE_NAME",OracleDbType.Varchar2,model.CREATE_NAME),
                //ServiceUtils.CreateOracleParameter(":CREATE_DATE",OracleDbType.Date,model.CREATE_DATE),
                ServiceUtils.CreateOracleParameter(":NOTE",OracleDbType.Varchar2,model.NOTE)
            });
            DbHelperOra.ExecuteSql(strsql.ToString(), parameters.ToArray());
            return true;
        }

        [WebMethod(EnableSession = true)]
        public bool Save_测井任务通知单(byte[] data_测井任务通知单, byte[] dataChangedList_预测项目信息 = null, byte[] dataRemoveList_预测项目信息 = null)
        {
            if (data_测井任务通知单 == null) return false;
            var model_测井任务通知单 = Utility.ModelHelper.DeserializeObject(data_测井任务通知单) as Model.DM_LOG_TASK;
            if (string.IsNullOrWhiteSpace(model_测井任务通知单.DRILL_JOB_ID)) return false;

            Workflow.Controller.ValidateSave<Workflow.C测井任务通知单>(model_测井任务通知单.REQUISITION_CD);

            StringBuilder strSql = new StringBuilder();
            List<OracleParameter> parameters = new List<OracleParameter>();
            strSql.Append("DECLARE items_id NUMBER(38,0); begin ");
            if (model_测井任务通知单.PREDICTED_LOGGING_ITEMS_ID == null)
            {
                strSql.Append("select logging_items_id_sequence.nextval into items_id from dual;");
            }
            else
            {
                strSql.Append("select " + model_测井任务通知单.PREDICTED_LOGGING_ITEMS_ID + " into items_id from dual;");
            }
            if (model_测井任务通知单.PREDICTED_LOGGING_ITEMS_ID == null)
            {
                if (model_测井任务通知单.TREATMENT_DATE_REQUISITION == null || model_测井任务通知单.TREATMENT_DATE_REQUISITION.Value.Year < 1949)
                    ServiceUtils.ThrowSoapException("通知单来电处理日期不正确！");
                model_测井任务通知单.REQUISITION_CD = model_测井任务通知单.WELL_JOB_NAME + "-" + DbHelperOra.GetSingle("select fn_no_make('DM_LOG_TASK','REQUISITION_CD',:TREATMENT_DATE_REQUISITION) from dual",
                    ServiceUtils.CreateOracleParameter(":TREATMENT_DATE_REQUISITION", OracleDbType.Date, model_测井任务通知单.TREATMENT_DATE_REQUISITION)) as string;
                //strSql.Append("select :WELL_JOB_NAME||'-'||fn_no_make('DM_LOG_TASK','REQUISITION_CD',:TREATMENT_DATE_REQUISITION) into requisition_cd from dual;");
                strSql.Append("insert into DM_LOG_TASK(");
                strSql.Append("REQUISITION_CD,REQUISITION_TYPE,PREDICTED_LOGGING_DATE,PREDICTED_LOGGING_INTERVAL,PREDICTED_LOGGING_ITEMS_ID,DEPARTMENT_REQUISITION,PERSON_EQUISITION,REC_NOTICE_TIME,RECIPIENT_REQUISITION,COMPLETE_MAN,TREATMENT_DATE_REQUISITION,MARKET_IDENTIFICATION,MARKET_CLASSIFY,VERIFIER,BRIEF_DESCRIPTION_CONTENT,NOTE,DRILL_STATUSE,DRILL_JOB_ID,REQUISITION_SCANNING_FILEID");
                strSql.Append(") values (");
                strSql.Append(":REQUISITION_CD,:REQUISITION_TYPE,:PREDICTED_LOGGING_DATE,:PREDICTED_LOGGING_INTERVAL,items_id,:DEPARTMENT_REQUISITION,:PERSON_EQUISITION,:REC_NOTICE_TIME,:RECIPIENT_REQUISITION,:COMPLETE_MAN,:TREATMENT_DATE_REQUISITION,:MARKET_IDENTIFICATION,:MARKET_CLASSIFY,:VERIFIER,:BRIEF_DESCRIPTION_CONTENT,:NOTE,:DRILL_STATUSE,:DRILL_JOB_ID,:REQUISITION_SCANNING_FILEID");
                strSql.Append(") ;");
                strSql.Append("insert into SYS_WORK_FLOW(");
                strSql.Append("FLOW_ID,OBJ_ID,SOURCE_LOGINNAME,FLOW_TYPE,FLOW_STATE");
                strSql.Append(") values (");
                strSql.Append("FLOW_ID_SEQ.NEXTVAL,:REQUISITION_CD,:SOURCE_LOGINNAME," + (int)ServiceEnums.WorkflowType.测井任务通知单 + "," + (int)ServiceEnums.WorkflowState.新建);
                strSql.Append(");");
                parameters.Add(ServiceUtils.CreateOracleParameter(":SOURCE_LOGINNAME", OracleDbType.Varchar2, ServiceUtils.GetUserInfo().COL_LOGINNAME));
                //parameters.Add();
                parameters.Add(ServiceUtils.CreateOracleParameter(":DRILL_JOB_ID", OracleDbType.Char, model_测井任务通知单.DRILL_JOB_ID));
            }
            else
            {
                strSql.Append("update DM_LOG_TASK set ");

                //strSql.Append(" REQUISITION_CD = :REQUISITION_CD , ");
                strSql.Append(" REQUISITION_TYPE = :REQUISITION_TYPE , ");
                strSql.Append(" PREDICTED_LOGGING_DATE = :PREDICTED_LOGGING_DATE , ");
                strSql.Append(" PREDICTED_LOGGING_INTERVAL = :PREDICTED_LOGGING_INTERVAL , ");
                //strSql.Append(" PREDICTED_LOGGING_ITEMS_ID = :PREDICTED_LOGGING_ITEMS_ID , ");
                strSql.Append(" DEPARTMENT_REQUISITION = :DEPARTMENT_REQUISITION , ");
                strSql.Append(" PERSON_EQUISITION = :PERSON_EQUISITION , ");
                strSql.Append(" REC_NOTICE_TIME = :REC_NOTICE_TIME , ");
                strSql.Append(" RECIPIENT_REQUISITION = :RECIPIENT_REQUISITION , ");
                strSql.Append(" COMPLETE_MAN = :COMPLETE_MAN , ");
                strSql.Append(" TREATMENT_DATE_REQUISITION = :TREATMENT_DATE_REQUISITION , ");
                strSql.Append(" MARKET_IDENTIFICATION = :MARKET_IDENTIFICATION , ");
                strSql.Append(" MARKET_CLASSIFY = :MARKET_CLASSIFY , ");
                strSql.Append(" VERIFIER = :VERIFIER , ");
                strSql.Append(" BRIEF_DESCRIPTION_CONTENT = :BRIEF_DESCRIPTION_CONTENT , ");
                strSql.Append(" REQUISITION_SCANNING_FILEID = :REQUISITION_SCANNING_FILEID , ");
                strSql.Append(" NOTE = :NOTE , ");
                strSql.Append(" DRILL_STATUSE = :DRILL_STATUSE ");
                //strSql.Append(" WELL_ID = :WELL_ID , ");
                //strSql.Append(" DRILL_JOB_ID = :DRILL_JOB_ID  ");
                strSql.Append(" where REQUISITION_CD=:REQUISITION_CD; ");
                //parameters.Add(ServiceUtils.CreateOracleParameter(":REQUISITION_CD", OracleDbType.Varchar2, model_测井任务通知单.REQUISITION_CD));
            }
            parameters.AddRange(new OracleParameter[] {
			            ServiceUtils.CreateOracleParameter(":REQUISITION_CD", OracleDbType.Varchar2, model_测井任务通知单.REQUISITION_CD) ,            
                        ServiceUtils.CreateOracleParameter(":REQUISITION_TYPE", OracleDbType.Decimal, model_测井任务通知单.REQUISITION_TYPE) ,            
                        ServiceUtils.CreateOracleParameter(":PREDICTED_LOGGING_DATE", OracleDbType.Date, model_测井任务通知单.PREDICTED_LOGGING_DATE) ,            
                        ServiceUtils.CreateOracleParameter(":PREDICTED_LOGGING_INTERVAL", OracleDbType.NVarchar2, model_测井任务通知单.PREDICTED_LOGGING_INTERVAL) ,            
                        //ServiceUtils.CreateOracleParameter(":PREDICTED_LOGGING_ITEMS_ID", OracleDbType.Decimal, model_测井任务通知单.PREDICTED_LOGGING_ITEMS_ID) ,            
                        ServiceUtils.CreateOracleParameter(":DEPARTMENT_REQUISITION", OracleDbType.Decimal, model_测井任务通知单.DEPARTMENT_REQUISITION) ,            
                        ServiceUtils.CreateOracleParameter(":PERSON_EQUISITION", OracleDbType.Varchar2, model_测井任务通知单.PERSON_EQUISITION) ,            
                        ServiceUtils.CreateOracleParameter(":REC_NOTICE_TIME", OracleDbType.Date, model_测井任务通知单.REC_NOTICE_TIME) ,            
                        ServiceUtils.CreateOracleParameter(":RECIPIENT_REQUISITION", OracleDbType.Varchar2, model_测井任务通知单.RECIPIENT_REQUISITION) ,            
                        ServiceUtils.CreateOracleParameter(":COMPLETE_MAN", OracleDbType.Varchar2, model_测井任务通知单.COMPLETE_MAN) ,            
                        ServiceUtils.CreateOracleParameter(":TREATMENT_DATE_REQUISITION", OracleDbType.Date, model_测井任务通知单.TREATMENT_DATE_REQUISITION) ,            
                        ServiceUtils.CreateOracleParameter(":MARKET_IDENTIFICATION", OracleDbType.Varchar2, model_测井任务通知单.MARKET_IDENTIFICATION) ,            
                        ServiceUtils.CreateOracleParameter(":MARKET_CLASSIFY", OracleDbType.Decimal, model_测井任务通知单.MARKET_CLASSIFY) ,            
                        ServiceUtils.CreateOracleParameter(":VERIFIER", OracleDbType.Varchar2, model_测井任务通知单.VERIFIER) ,            
                        ServiceUtils.CreateOracleParameter(":BRIEF_DESCRIPTION_CONTENT", OracleDbType.Varchar2, model_测井任务通知单.BRIEF_DESCRIPTION_CONTENT) ,            
                        ServiceUtils.CreateOracleParameter(":REQUISITION_SCANNING_FILEID", OracleDbType.Decimal, model_测井任务通知单.REQUISITION_SCANNING_FILEID) ,            
                        ServiceUtils.CreateOracleParameter(":NOTE", OracleDbType.NVarchar2, model_测井任务通知单.NOTE) ,            
                        ServiceUtils.CreateOracleParameter(":DRILL_STATUSE", OracleDbType.Varchar2, model_测井任务通知单.DRILL_STATUSE) ,            
                       // ServiceUtils.CreateOracleParameter(":WELL_ID", OracleDbType.Char, model_测井任务通知单.WELL_ID)             
                        //ServiceUtils.CreateOracleParameter(":DRILL_JOB_ID", OracleDbType.Varchar2,model_测井任务通知单.DRILL_JOB_ID),             
              
            });
            if (dataChangedList_预测项目信息 != null)
            {
                var model_dataChangedList = Utility.ModelHelper.DeserializeObject(dataChangedList_预测项目信息) as List<Model.DM_LOG_PREDICTED_ITEM>;
                if (model_dataChangedList != null)
                {
                    for (int i = 0; i < model_dataChangedList.Count; ++i)
                    {
                        Model.DM_LOG_PREDICTED_ITEM model = model_dataChangedList[i];
                        if (model.PREDICTED_LOGGING_ITEMS_ID == null)
                        {
                            strSql.Append("insert into DM_LOG_PREDICTED_ITEM(");
                            strSql.Append("PREDICTED_LOGGING_ITEMS_ID,PREDICTED_LOGGING_NAME,PRE_ST_DEP,PRE_EN_DEP,PRE_SCALE,NOTE,LOG_ITEM_ID");
                            strSql.Append(") values (");
                            strSql.Append("items_id,:PREDICTED_LOGGING_NAME" + i + ",:PRE_ST_DEP" + i + ",:PRE_EN_DEP" + i + ",:PRE_SCALE" + i + ",:NOTE" + i + ",:LOG_ITEM_ID" + i + "");
                            strSql.Append(") ;");
                        }
                        else
                        {
                            strSql.Append("update DM_LOG_PREDICTED_ITEM set ");
                            //strSql.Append(" PREDICTED_LOGGING_ITEMS_ID = :PREDICTED_LOGGING_ITEMS_ID , ");
                            strSql.Append(" PREDICTED_LOGGING_NAME = :PREDICTED_LOGGING_NAME" + i + ", ");
                            strSql.Append(" PRE_ST_DEP = :PRE_ST_DEP" + i + " , ");
                            strSql.Append(" PRE_EN_DEP = :PRE_EN_DEP" + i + " , ");
                            strSql.Append(" PRE_SCALE = :PRE_SCALE" + i + " , ");
                            strSql.Append(" NOTE = :NOTE" + i + " ");
                            //strSql.Append(" LOG_ITEM_ID = :LOG_ITEM_ID" + i + "  ");
                            strSql.Append(" where PREDICTED_LOGGING_ITEMS_ID=items_id and LOG_ITEM_ID=:LOG_ITEM_ID" + i + ";");
                        }


                        parameters.AddRange(new OracleParameter[]{
			            //ServiceUtils.CreateOracleParameter(":PREDICTED_LOGGING_ITEMS_ID", OracleDbType.Decimal, model.PREDICTED_LOGGING_ITEMS_ID) ,            
                        ServiceUtils.CreateOracleParameter(":PREDICTED_LOGGING_NAME" + i, OracleDbType.NVarchar2, model.PREDICTED_LOGGING_NAME),            
                        ServiceUtils.CreateOracleParameter(":PRE_ST_DEP"+i, OracleDbType.Decimal, model.PRE_ST_DEP) ,            
                        ServiceUtils.CreateOracleParameter(":PRE_EN_DEP"+i, OracleDbType.Decimal, model.PRE_EN_DEP) ,            
                        ServiceUtils.CreateOracleParameter(":PRE_SCALE"+i, OracleDbType.NVarchar2, model.PRE_SCALE) ,            
                        ServiceUtils.CreateOracleParameter(":NOTE"+i, OracleDbType.NVarchar2, model.NOTE) ,            
                        ServiceUtils.CreateOracleParameter(":LOG_ITEM_ID"+i, OracleDbType.Decimal, model.LOG_ITEM_ID) });
                    }
                }
            }

            if (dataRemoveList_预测项目信息 != null)
            {
                var model_dataRemoveList = Utility.ModelHelper.DeserializeObject(dataRemoveList_预测项目信息) as List<Model.DM_LOG_PREDICTED_ITEM>;
                if (model_dataRemoveList != null)
                {
                    foreach (Model.DM_LOG_PREDICTED_ITEM model in model_dataRemoveList)
                    {
                        strSql.Append("delete from DM_LOG_PREDICTED_ITEM ");
                        strSql.Append("where PREDICTED_LOGGING_ITEMS_ID=items_id and LOG_ITEM_ID=" + model.LOG_ITEM_ID);
                        strSql.Append(";");
                    }
                }
            }
            strSql.Append(" end;");
            int r = DbHelperOra.ExecuteSqlTran(strSql.ToString(), parameters.ToArray());
            if (r > 0) return true;
            return false;
        }
        [WebMethod(EnableSession = true)]
        public DataSet GetTeamList()
        {
            return DbHelperOra.Query("select team_org_id from pkl_log_team order by team_org_id");
        }
        [WebMethod(EnableSession = true)]

        public DataSet GetLogType()
        {
            return DbHelperOra.Query("SELECT LOG_TYPE_NAME FROM PKL_LOG_TYPE ORDER BY LOG_TYPE_ID");
        }
        [WebMethod(EnableSession = true)]

        public DataSet GetLogMode()
        {
            return DbHelperOra.Query("SELECT CONSTRUCTION_TECHNOLOGY_NAME FROM PKL_LOG_OPS_TECHNOLOGY ORDER BY CONSTRUCTION_TECHNOLOGY_ID");
        }

        [WebMethod(EnableSession = true)]
        public bool Save_测井任务计划书(byte[] data_测井任务计划书)
        {

            if (data_测井任务计划书 == null) return false;
            Model.DM_LOG_OPS_PLAN model = Utility.ModelHelper.DeserializeObject(data_测井任务计划书) as Model.DM_LOG_OPS_PLAN;
            if (string.IsNullOrWhiteSpace(model.REQUISITION_CD)) return false;
            if (string.IsNullOrWhiteSpace(model.LOG_TEAM_ID)) ServiceUtils.ThrowSoapException("作业小队不能为空！");

            Workflow.Controller.ValidateSave<Workflow.C计划任务书>(model.JOB_PLAN_CD, model.REQUISITION_CD);

            StringBuilder strSql = new StringBuilder();
            List<OracleParameter> parameters = new List<OracleParameter>();
            if (string.IsNullOrWhiteSpace(model.JOB_PLAN_CD))
            {
                model.JOB_PLAN_CD = model.REQUISITION_CD + "-" + model.LOG_TEAM_ID;
                if (DbHelperOra.Exists("select 1 from DM_LOG_OPS_PLAN where job_plan_cd=:JOB_PLAN_CD",
                    ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, model.JOB_PLAN_CD)))
                    ServiceUtils.ThrowSoapException("测井任务计划书“" + model.JOB_PLAN_CD + "”已经存在！");
                strSql.Append("begin ");
                strSql.Append("insert into DM_LOG_OPS_PLAN(");
                strSql.Append("REQUISITION_CD,PREPARE_PERSON,VERIFIER,APPROVER,PREPARE_DATE,PLAN_CONTENT_SCANNING,NOTE,LOG_TEAM_ID,JOB_PLAN_CD,RECEIVED_INFORM_TIME,REQUIREMENTS_TIME,JOB_ID,JOB_LAYER,LOG_TYPE,LOG_MODE,PRELOGGING_INTERVAL,PLAN_CONTENT_SCANNING_FILEID");
                strSql.Append(") values (");
                strSql.Append(":REQUISITION_CD,:PREPARE_PERSON,:VERIFIER,:APPROVER,:PREPARE_DATE,:PLAN_CONTENT_SCANNING,:NOTE,:LOG_TEAM_ID,:JOB_PLAN_CD,:RECEIVED_INFORM_TIME,:REQUIREMENTS_TIME,:JOB_ID,:JOB_LAYER,:LOG_TYPE,:LOG_MODE,:PRELOGGING_INTERVAL,:PLAN_CONTENT_SCANNING_FILEID");
                strSql.Append(");");
                strSql.Append("insert into SYS_WORK_FLOW(");
                strSql.Append("FLOW_ID,OBJ_ID,SOURCE_LOGINNAME,FLOW_TYPE,FLOW_STATE");
                strSql.Append(") values (");
                strSql.Append("FLOW_ID_SEQ.NEXTVAL,:JOB_PLAN_CD,:SOURCE_LOGINNAME," + (int)ServiceEnums.WorkflowType.计划任务书 + "," + (int)ServiceEnums.WorkflowState.新建);
                strSql.Append(");");
                strSql.Append(" end;");
                parameters.Add(ServiceUtils.CreateOracleParameter(":SOURCE_LOGINNAME", OracleDbType.Varchar2, ServiceUtils.GetUserInfo().COL_LOGINNAME));
                parameters.Add(ServiceUtils.CreateOracleParameter(":REQUISITION_CD", OracleDbType.Varchar2, model.REQUISITION_CD));
                parameters.Add(ServiceUtils.CreateOracleParameter(":LOG_TEAM_ID", OracleDbType.Varchar2, model.LOG_TEAM_ID));
            }
            else
            {
                strSql.Append("update DM_LOG_OPS_PLAN set ");
                //strSql.Append(" REQUISITION_CD = :REQUISITION_CD , ");
                strSql.Append(" PREPARE_PERSON = :PREPARE_PERSON , ");
                strSql.Append(" VERIFIER = :VERIFIER , ");
                strSql.Append(" APPROVER = :APPROVER , ");
                strSql.Append(" PREPARE_DATE = :PREPARE_DATE , ");
                strSql.Append(" PLAN_CONTENT_SCANNING = :PLAN_CONTENT_SCANNING , ");
                strSql.Append(" NOTE = :NOTE , ");
                //strSql.Append(" LOG_TEAM_ID = :LOG_TEAM_ID , ");
                //strSql.Append(" JOB_PLAN_CD = :JOB_PLAN_CD , ");
                strSql.Append(" RECEIVED_INFORM_TIME = :RECEIVED_INFORM_TIME , ");
                strSql.Append(" REQUIREMENTS_TIME = :REQUIREMENTS_TIME , ");
                strSql.Append(" JOB_ID = :JOB_ID , ");
                strSql.Append(" JOB_LAYER = :JOB_LAYER , ");
                //strSql.Append(" PREDICTED_LOGGING_ITEMS_ID = :PREDICTED_LOGGING_ITEMS_ID , ");
                strSql.Append(" LOG_TYPE = :LOG_TYPE , ");
                strSql.Append(" LOG_MODE = :LOG_MODE ,");
                strSql.Append(" PRELOGGING_INTERVAL = :PRELOGGING_INTERVAL,");
                strSql.Append(" PLAN_CONTENT_SCANNING_FILEID = :PLAN_CONTENT_SCANNING_FILEID ");
                strSql.Append(" where JOB_PLAN_CD=:JOB_PLAN_CD  ");
            }
            parameters.AddRange(new OracleParameter[] {
			            ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, model.JOB_PLAN_CD),
                        ServiceUtils.CreateOracleParameter(":PREPARE_PERSON", OracleDbType.Varchar2, model.PREPARE_PERSON) ,            
                        ServiceUtils.CreateOracleParameter(":VERIFIER", OracleDbType.Varchar2, model.VERIFIER) ,            
                        ServiceUtils.CreateOracleParameter(":APPROVER", OracleDbType.Varchar2, model.APPROVER) ,            
                        ServiceUtils.CreateOracleParameter(":PREPARE_DATE", OracleDbType.Date, model.PREPARE_DATE) ,            
                        ServiceUtils.CreateOracleParameter(":PLAN_CONTENT_SCANNING", OracleDbType.Blob, model.PLAN_CONTENT_SCANNING) ,            
                        ServiceUtils.CreateOracleParameter(":NOTE", OracleDbType.Varchar2, model.NOTE) ,            
                        //ServiceUtils.CreateOracleParameter(":LOG_TEAM_ID", OracleDbType.Varchar2, model.LOG_TEAM_ID) ,                 
                        ServiceUtils.CreateOracleParameter(":RECEIVED_INFORM_TIME", OracleDbType.Date, model.RECEIVED_INFORM_TIME) ,            
                        ServiceUtils.CreateOracleParameter(":REQUIREMENTS_TIME", OracleDbType.Date, model.REQUIREMENTS_TIME) ,            
                        ServiceUtils.CreateOracleParameter(":JOB_ID", OracleDbType.Decimal, model.JOB_ID) ,            
                        ServiceUtils.CreateOracleParameter(":JOB_LAYER", OracleDbType.NVarchar2, model.JOB_LAYER) ,            
                        //ServiceUtils.CreateOracleParameter(":PREDICTED_LOGGING_ITEMS_ID", OracleDbType.NVarchar2, model.PREDICTED_LOGGING_ITEMS_ID) ,            
                        ServiceUtils.CreateOracleParameter(":LOG_TYPE", OracleDbType.Varchar2, model.LOG_TYPE) ,            
                        ServiceUtils.CreateOracleParameter(":LOG_MODE", OracleDbType.Varchar2, model.LOG_MODE) ,
                        ServiceUtils.CreateOracleParameter(":PRELOGGING_INTERVAL",OracleDbType.Varchar2,model.PRELOGGING_INTERVAL),            
                        ServiceUtils.CreateOracleParameter(":PLAN_CONTENT_SCANNING_FILEID", OracleDbType.Decimal, model.PLAN_CONTENT_SCANNING_FILEID)
            });
            int i = DbHelperOra.ExecuteSqlTran(strSql.ToString(), parameters.ToArray());
            if (i > 0) return true;
            return false;
        }
        [WebMethod(EnableSession = true)]
        public DataSet GetComboBoxList_钻井状态()
        {
            return DbHelperOra.Query("select status_name from pkl_log_drill_status order by rowid");
        }
        [WebMethod(EnableSession = true)]
        public bool Save_小队施工基本信息(byte[] data_小队施工基本信息, byte[] data_小队施工时效, byte[] data_小队施工人员, byte[] data_小队施工地面设备)
        {
            if (data_小队施工基本信息 == null || data_小队施工时效 == null || data_小队施工人员 == null || data_小队施工地面设备 == null) return false;
            Model.DM_LOG_BASE model_小队施工基本信息 = Utility.ModelHelper.DeserializeObject(data_小队施工基本信息) as Model.DM_LOG_BASE;
            if (string.IsNullOrWhiteSpace(model_小队施工基本信息.JOB_PLAN_CD)) return false;

            Model.DM_LOG_WORK_ANING model_小队施工时效 = Utility.ModelHelper.DeserializeObject(data_小队施工时效) as Model.DM_LOG_WORK_ANING;
            if (string.IsNullOrWhiteSpace(model_小队施工时效.REQUISITION_CD)) return false;

            Workflow.Controller.ValidateSave<Workflow.C测井现场提交信息>(model_小队施工基本信息.JOB_PLAN_CD);

            Model.DM_LOG_WORK_PERSONNEL model_小队施工人员 = Utility.ModelHelper.DeserializeObject(data_小队施工人员) as Model.DM_LOG_WORK_PERSONNEL;
            Model.DM_LOG_UP_EQUIP model_小队施工地面设备 = Utility.ModelHelper.DeserializeObject(data_小队施工地面设备) as Model.DM_LOG_UP_EQUIP;
            StringBuilder strSql = new StringBuilder();
            List<OracleParameter> parameters = new List<OracleParameter>();
            strSql.Append("begin ");
            strSql.Append("update DM_LOG_TASK set ");
            strSql.Append(" DRILL_STATUSE = :DRILL_STATUSE  ");
            strSql.Append(" where REQUISITION_CD=:REQUISITION_CD;");
            parameters.AddRange(new OracleParameter[] {
                ServiceUtils.CreateOracleParameter(":REQUISITION_CD", OracleDbType.Varchar2, model_小队施工时效.REQUISITION_CD),
                ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, model_小队施工基本信息.JOB_PLAN_CD),
			    ServiceUtils.CreateOracleParameter(":DRILL_STATUSE", OracleDbType.Varchar2, model_小队施工基本信息.DRILL_STATUSE)             
            });

            if (string.IsNullOrWhiteSpace(model_小队施工基本信息.BASEID))
            {
                strSql.Append("insert into DM_LOG_BASE(");
                strSql.Append("BASEID,JOB_PROFILE_SERIES,MAXIMUM_SLOPE,MAXIMUM_SLOPE_DEPTH,MUD_RESITIVITY,MUD_TEMERATURE,BOTTOM_TEMPERATURE,BOTTOM_DEP,WELLBORE_FLUID,START_DEPTH,END_DEPTH,JOB_PLAN_CD,IS_COMPLETE,LOG_TYPE,LOG_MODE,INSTRUMENT_COUNT,INSTRUMENT_SUCCESS,WAVE_SELECTION,NOTE,JOB_PURPOSE,COLLABORATION_DEPARTMENT,CONSTRUCTION_ORG,RECEIVED_INFORM_TIME,P_XN_DRILL_DEPTH,P_XN_LOG_DEPTH,JOB_LAYER");
                strSql.Append(") values (");
                strSql.Append("LOG_BASEID_SEQUENCE.nextval,:JOB_PROFILE_SERIES,:MAXIMUM_SLOPE,:MAXIMUM_SLOPE_DEPTH,:MUD_RESITIVITY,:MUD_TEMERATURE,:BOTTOM_TEMPERATURE,:BOTTOM_DEP,:WELLBORE_FLUID,:START_DEPTH,:END_DEPTH,:JOB_PLAN_CD,:IS_COMPLETE,:LOG_TYPE,:LOG_MODE,:INSTRUMENT_COUNT,:INSTRUMENT_SUCCESS,:WAVE_SELECTION,:NOTE,:JOB_PURPOSE,:COLLABORATION_DEPARTMENT,:CONSTRUCTION_ORG,:RECEIVED_INFORM_TIME,:P_XN_DRILL_DEPTH,:P_XN_LOG_DEPTH,:JOB_LAYER");
                strSql.Append(") ;");
            }
            else
            {
                strSql.Append("update DM_LOG_BASE set ");
                //strSql.Append(" BASEID = :BASEID , ");
                strSql.Append(" JOB_PROFILE_SERIES = :JOB_PROFILE_SERIES , ");
                strSql.Append(" MAXIMUM_SLOPE = :MAXIMUM_SLOPE , ");
                strSql.Append(" MAXIMUM_SLOPE_DEPTH = :MAXIMUM_SLOPE_DEPTH , ");
                strSql.Append(" MUD_RESITIVITY = :MUD_RESITIVITY , ");
                strSql.Append(" MUD_TEMERATURE = :MUD_TEMERATURE , ");
                strSql.Append(" BOTTOM_TEMPERATURE = :BOTTOM_TEMPERATURE , ");
                strSql.Append(" BOTTOM_DEP = :BOTTOM_DEP , ");
                strSql.Append(" WELLBORE_FLUID = :WELLBORE_FLUID , ");
                strSql.Append(" START_DEPTH = :START_DEPTH , ");
                strSql.Append(" END_DEPTH = :END_DEPTH , ");
                //strSql.Append(" JOB_PLAN_CD = :JOB_PLAN_CD , ");
                strSql.Append(" IS_COMPLETE = :IS_COMPLETE , ");
                strSql.Append(" LOG_TYPE = :LOG_TYPE , ");
                strSql.Append(" LOG_MODE = :LOG_MODE , ");
                strSql.Append(" INSTRUMENT_COUNT = :INSTRUMENT_COUNT , ");
                strSql.Append(" INSTRUMENT_SUCCESS = :INSTRUMENT_SUCCESS , ");
                strSql.Append(" WAVE_SELECTION = :WAVE_SELECTION , ");
                strSql.Append(" NOTE = :NOTE , ");
                strSql.Append(" JOB_PURPOSE = :JOB_PURPOSE , ");
                strSql.Append(" COLLABORATION_DEPARTMENT = :COLLABORATION_DEPARTMENT , ");
                strSql.Append(" CONSTRUCTION_ORG = :CONSTRUCTION_ORG , ");
                strSql.Append(" RECEIVED_INFORM_TIME = :RECEIVED_INFORM_TIME , ");
                strSql.Append(" P_XN_DRILL_DEPTH = :P_XN_DRILL_DEPTH , ");
                strSql.Append(" P_XN_LOG_DEPTH = :P_XN_LOG_DEPTH , ");
                strSql.Append(" JOB_LAYER = :JOB_LAYER  ");
                strSql.Append(" where BASEID=:BASEID  ;");
                parameters.Add(ServiceUtils.CreateOracleParameter(":BASEID", OracleDbType.Varchar2, model_小队施工基本信息.BASEID));
            }

            parameters.AddRange(new OracleParameter[] {           
                        ServiceUtils.CreateOracleParameter(":JOB_PROFILE_SERIES", OracleDbType.NVarchar2, model_小队施工基本信息.JOB_PROFILE_SERIES) ,            
                        ServiceUtils.CreateOracleParameter(":MAXIMUM_SLOPE", OracleDbType.Decimal, model_小队施工基本信息.MAXIMUM_SLOPE) ,            
                        ServiceUtils.CreateOracleParameter(":MAXIMUM_SLOPE_DEPTH", OracleDbType.Decimal, model_小队施工基本信息.MAXIMUM_SLOPE_DEPTH) ,            
                        ServiceUtils.CreateOracleParameter(":MUD_RESITIVITY", OracleDbType.Decimal, model_小队施工基本信息.MUD_RESITIVITY) ,            
                        ServiceUtils.CreateOracleParameter(":MUD_TEMERATURE", OracleDbType.Decimal, model_小队施工基本信息.MUD_TEMERATURE) ,            
                        ServiceUtils.CreateOracleParameter(":BOTTOM_TEMPERATURE", OracleDbType.Decimal, model_小队施工基本信息.BOTTOM_TEMPERATURE) ,            
                        ServiceUtils.CreateOracleParameter(":BOTTOM_DEP", OracleDbType.Decimal, model_小队施工基本信息.BOTTOM_DEP) ,            
                        ServiceUtils.CreateOracleParameter(":WELLBORE_FLUID", OracleDbType.Varchar2, model_小队施工基本信息.WELLBORE_FLUID) ,            
                        ServiceUtils.CreateOracleParameter(":START_DEPTH", OracleDbType.Decimal, model_小队施工基本信息.START_DEPTH) ,            
                        ServiceUtils.CreateOracleParameter(":END_DEPTH", OracleDbType.Decimal, model_小队施工基本信息.END_DEPTH) ,            
                        //ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, model_小队施工基本信息.JOB_PLAN_CD) ,    
                        ServiceUtils.CreateOracleParameter(":IS_COMPLETE", OracleDbType.Varchar2, model_小队施工基本信息.IS_COMPLETE) ,            
                        ServiceUtils.CreateOracleParameter(":LOG_TYPE", OracleDbType.Varchar2, model_小队施工基本信息.LOG_TYPE) ,            
                        ServiceUtils.CreateOracleParameter(":LOG_MODE", OracleDbType.Varchar2, model_小队施工基本信息.LOG_MODE) ,            
                        ServiceUtils.CreateOracleParameter(":INSTRUMENT_COUNT", OracleDbType.Decimal, model_小队施工基本信息.INSTRUMENT_COUNT) ,            
                        ServiceUtils.CreateOracleParameter(":INSTRUMENT_SUCCESS", OracleDbType.Decimal, model_小队施工基本信息.INSTRUMENT_SUCCESS) ,            
                        ServiceUtils.CreateOracleParameter(":WAVE_SELECTION", OracleDbType.Varchar2, model_小队施工基本信息.WAVE_SELECTION) ,            
                        ServiceUtils.CreateOracleParameter(":NOTE", OracleDbType.Varchar2, model_小队施工基本信息.NOTE) ,            
                        ServiceUtils.CreateOracleParameter(":JOB_PURPOSE", OracleDbType.NVarchar2, model_小队施工基本信息.JOB_PURPOSE) ,            
                        ServiceUtils.CreateOracleParameter(":COLLABORATION_DEPARTMENT", OracleDbType.NVarchar2, model_小队施工基本信息.COLLABORATION_DEPARTMENT) ,            
                        ServiceUtils.CreateOracleParameter(":CONSTRUCTION_ORG", OracleDbType.NVarchar2, model_小队施工基本信息.CONSTRUCTION_ORG) ,            
                        ServiceUtils.CreateOracleParameter(":RECEIVED_INFORM_TIME", OracleDbType.Date, model_小队施工基本信息.RECEIVED_INFORM_TIME) ,            
                        ServiceUtils.CreateOracleParameter(":P_XN_DRILL_DEPTH", OracleDbType.Decimal, model_小队施工基本信息.P_XN_DRILL_DEPTH) ,            
                        ServiceUtils.CreateOracleParameter(":P_XN_LOG_DEPTH", OracleDbType.Decimal, model_小队施工基本信息.P_XN_LOG_DEPTH) ,            
                        ServiceUtils.CreateOracleParameter(":JOB_LAYER", OracleDbType.NVarchar2, model_小队施工基本信息.JOB_LAYER)
                });
            if (model_小队施工时效.ANINGID == null)
            {
                strSql.Append("insert into DM_LOG_WORK_ANING(");
                strSql.Append("ANINGID,REQUISITION_CD,JOB_PLAN_CD,RECEIVED_INFORM_TIME,REQUIREMENTS_TIME,ARRIVE_TIME,RECEIVING_TIME,HAND_TIME,LEAVE_TIME,WAIT_TIME,LOG_START_TIME,LOG_END_TIME,LOG_TOTAL_TIME,LOST_TIME,WINCH_RUNNING_TIME,DEPARTURE_POINT_,UNILATERAL_DISTANCE,DEPARTURE_POINT_TIME,RETURN_POINT,RETURN_POINT_TIME");
                strSql.Append(") values (");
                strSql.Append("LOG_ANINGID_SEQUENCE.nextval,:REQUISITION_CD,:JOB_PLAN_CD,:RECEIVED_INFORM_TIME,:REQUIREMENTS_TIME,:ARRIVE_TIME,:RECEIVING_TIME,:HAND_TIME,:LEAVE_TIME,:WAIT_TIME,:LOG_START_TIME,:LOG_END_TIME,:LOG_TOTAL_TIME,:LOST_TIME,:WINCH_RUNNING_TIME,:DEPARTURE_POINT_,:UNILATERAL_DISTANCE,:DEPARTURE_POINT_TIME,:RETURN_POINT,:RETURN_POINT_TIME");
                strSql.Append(");");
                //parameters.Add(ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, string_通知单编码));
            }
            else
            {
                strSql.Append("update DM_LOG_WORK_ANING set ");
                //strSql.Append(" ANINGID = :ANINGID , ");
                //strSql.Append(" REQUISITION_CD = :REQUISITION_CD , ");
                //strSql.Append(" JOB_PLAN_CD = :JOB_PLAN_CD , ");
                strSql.Append(" RECEIVED_INFORM_TIME = :RECEIVED_INFORM_TIME , ");
                strSql.Append(" REQUIREMENTS_TIME = :REQUIREMENTS_TIME , ");
                strSql.Append(" ARRIVE_TIME = :ARRIVE_TIME , ");
                strSql.Append(" RECEIVING_TIME = :RECEIVING_TIME , ");
                strSql.Append(" HAND_TIME = :HAND_TIME , ");
                strSql.Append(" LEAVE_TIME = :LEAVE_TIME , ");
                strSql.Append(" WAIT_TIME = :WAIT_TIME , ");
                strSql.Append(" LOG_START_TIME = :LOG_START_TIME , ");
                strSql.Append(" LOG_END_TIME = :LOG_END_TIME , ");
                strSql.Append(" LOG_TOTAL_TIME = :LOG_TOTAL_TIME , ");
                strSql.Append(" LOST_TIME = :LOST_TIME , ");
                strSql.Append(" WINCH_RUNNING_TIME = :WINCH_RUNNING_TIME ,");
                strSql.Append(" DEPARTURE_POINT_ = :DEPARTURE_POINT_ , ");
                strSql.Append(" UNILATERAL_DISTANCE = :UNILATERAL_DISTANCE , ");
                strSql.Append(" DEPARTURE_POINT_TIME = :DEPARTURE_POINT_TIME , ");
                strSql.Append(" RETURN_POINT = :RETURN_POINT , ");
                strSql.Append(" RETURN_POINT_TIME = :RETURN_POINT_TIME ");
                strSql.Append(" where ANINGID=:ANINGID ; ");
                parameters.Add(ServiceUtils.CreateOracleParameter(":ANINGID", OracleDbType.Decimal, model_小队施工时效.ANINGID));
            }
            parameters.AddRange(new OracleParameter[] {          
                        //ServiceUtils.CreateOracleParameter(":REQUISITION_CD", OracleDbType.Varchar2, model_小队施工时效.REQUISITION_CD) ,                    
                        ServiceUtils.CreateOracleParameter(":RECEIVED_INFORM_TIME", OracleDbType.Date, model_小队施工时效.RECEIVED_INFORM_TIME) ,            
                        ServiceUtils.CreateOracleParameter(":REQUIREMENTS_TIME", OracleDbType.Date, model_小队施工时效.REQUIREMENTS_TIME) ,            
                        ServiceUtils.CreateOracleParameter(":ARRIVE_TIME", OracleDbType.Date, model_小队施工时效.ARRIVE_TIME) ,            
                        ServiceUtils.CreateOracleParameter(":RECEIVING_TIME", OracleDbType.Date, model_小队施工时效.RECEIVING_TIME) ,            
                        ServiceUtils.CreateOracleParameter(":HAND_TIME", OracleDbType.Date, model_小队施工时效.HAND_TIME) ,            
                        ServiceUtils.CreateOracleParameter(":LEAVE_TIME", OracleDbType.Date, model_小队施工时效.LEAVE_TIME) ,            
                        ServiceUtils.CreateOracleParameter(":WAIT_TIME", OracleDbType.Decimal, model_小队施工时效.WAIT_TIME) ,            
                        ServiceUtils.CreateOracleParameter(":LOG_START_TIME", OracleDbType.Date, model_小队施工时效.LOG_START_TIME) ,            
                        ServiceUtils.CreateOracleParameter(":LOG_END_TIME", OracleDbType.Date, model_小队施工时效.LOG_END_TIME) ,            
                        ServiceUtils.CreateOracleParameter(":LOG_TOTAL_TIME", OracleDbType.Decimal, model_小队施工时效.LOG_TOTAL_TIME) ,            
                        ServiceUtils.CreateOracleParameter(":LOST_TIME", OracleDbType.Decimal, model_小队施工时效.LOST_TIME) ,            
                        ServiceUtils.CreateOracleParameter(":WINCH_RUNNING_TIME", OracleDbType.Decimal, model_小队施工时效.WINCH_RUNNING_TIME) ,            
                        ServiceUtils.CreateOracleParameter(":DEPARTURE_POINT_", OracleDbType.Varchar2, model_小队施工时效.DEPARTURE_POINT_) ,            
                        ServiceUtils.CreateOracleParameter(":UNILATERAL_DISTANCE", OracleDbType.Decimal, model_小队施工时效.UNILATERAL_DISTANCE),  
                        ServiceUtils.CreateOracleParameter(":DEPARTURE_POINT_TIME",OracleDbType.Date,model_小队施工时效.DEPARTURE_POINT_TIME),
                        ServiceUtils.CreateOracleParameter(":RETURN_POINT",OracleDbType.Varchar2,model_小队施工时效.RETURN_POINT),
                        ServiceUtils.CreateOracleParameter(":RETURN_POINT_TIME",OracleDbType.Date,model_小队施工时效.RETURN_POINT_TIME)
            });

            if (string.IsNullOrWhiteSpace(model_小队施工人员.PERSONNEL_ID))
            {
                strSql.Append("insert into DM_LOG_WORK_PERSONNEL(");
                strSql.Append("PERSONNEL_ID,JOB_PLAN_CD,LOG_TEAM_LEADER,CONTACT_TELEPHONE,LOG_OPERATOR__NAME,SOURCE_OPERATOR_NAME,LOG_SUPERVISION_NAME,OTHER_CONSTRUCTION_PERSONS,FIELD_INSPECTOR,INDOOR_INSPECTOR,NOTE");
                strSql.Append(") values (");
                strSql.Append("PERSONNEL_ID_SEQUENCE.nextval,:JOB_PLAN_CD,:LOG_TEAM_LEADER,:CONTACT_TELEPHONE,:LOG_OPERATOR__NAME,:SOURCE_OPERATOR_NAME,:LOG_SUPERVISION_NAME,:OTHER_CONSTRUCTION_PERSONS,:FIELD_INSPECTOR,:INDOOR_INSPECTOR,:NOTE");
                strSql.Append(") ;");
            }
            else
            {
                strSql.Append("update DM_LOG_WORK_PERSONNEL set ");
                //strSql.Append(" PERSONNEL_ID = :PERSONNEL_ID , ");                                    
                //strSql.Append(" JOB_PLAN_CD = :JOB_PLAN_CD , ");                                    
                strSql.Append(" LOG_TEAM_LEADER = :LOG_TEAM_LEADER , ");
                strSql.Append(" CONTACT_TELEPHONE = :CONTACT_TELEPHONE , ");
                strSql.Append(" LOG_OPERATOR__NAME = :LOG_OPERATOR__NAME , ");
                strSql.Append(" SOURCE_OPERATOR_NAME = :SOURCE_OPERATOR_NAME , ");
                strSql.Append(" LOG_SUPERVISION_NAME = :LOG_SUPERVISION_NAME , ");
                strSql.Append(" OTHER_CONSTRUCTION_PERSONS = :OTHER_CONSTRUCTION_PERSONS , ");
                strSql.Append(" FIELD_INSPECTOR = :FIELD_INSPECTOR , ");
                strSql.Append(" INDOOR_INSPECTOR = :INDOOR_INSPECTOR , ");
                strSql.Append(" NOTE = :NOTE  ");
                strSql.Append(" where PERSONNEL_ID=:PERSONNEL_ID ;");
                parameters.Add(ServiceUtils.CreateOracleParameter(":PERSONNEL_ID", OracleDbType.Varchar2, model_小队施工人员.PERSONNEL_ID));
            }
            parameters.AddRange(new OracleParameter[] {          
                        //ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, model_小队施工人员.JOB_PLAN_CD) ,            
                        ServiceUtils.CreateOracleParameter(":LOG_TEAM_LEADER", OracleDbType.Varchar2, model_小队施工人员.LOG_TEAM_LEADER) ,            
                        ServiceUtils.CreateOracleParameter(":CONTACT_TELEPHONE", OracleDbType.Varchar2, model_小队施工人员.CONTACT_TELEPHONE) ,            
                        ServiceUtils.CreateOracleParameter(":LOG_OPERATOR__NAME", OracleDbType.Varchar2, model_小队施工人员.LOG_OPERATOR__NAME) ,            
                        ServiceUtils.CreateOracleParameter(":SOURCE_OPERATOR_NAME", OracleDbType.Varchar2, model_小队施工人员.SOURCE_OPERATOR_NAME) ,            
                        ServiceUtils.CreateOracleParameter(":LOG_SUPERVISION_NAME", OracleDbType.Varchar2, model_小队施工人员.LOG_SUPERVISION_NAME) ,            
                        ServiceUtils.CreateOracleParameter(":OTHER_CONSTRUCTION_PERSONS", OracleDbType.Varchar2, model_小队施工人员.OTHER_CONSTRUCTION_PERSONS) ,            
                        ServiceUtils.CreateOracleParameter(":FIELD_INSPECTOR", OracleDbType.Varchar2, model_小队施工人员.FIELD_INSPECTOR) ,            
                        ServiceUtils.CreateOracleParameter(":INDOOR_INSPECTOR", OracleDbType.Varchar2, model_小队施工人员.INDOOR_INSPECTOR)            
                        //ServiceUtils.CreateOracleParameter(":NOTE", OracleDbType.Varchar2, model_小队施工人员.NOTE)             
              
            });
            if (string.IsNullOrWhiteSpace(model_小队施工地面设备.REQUISITION_CD))
            {
                strSql.Append("insert into DM_LOG_UP_EQUIP(");
                strSql.Append("REQUISITION_CD,JOB_PLAN_CD,TEAM_ORG_ID,LOG_SERIES_ID,WINCH_LICENCEPLATE,WINCHC_ABLE_LENGTH,LOGGINGTRUCK_LICENSEPLATE");
                strSql.Append(") values (");
                strSql.Append(":REQUISITION_CD,:JOB_PLAN_CD,:TEAM_ORG_ID,:LOG_SERIES_ID,:WINCH_LICENCEPLATE,:WINCHC_ABLE_LENGTH,:LOGGINGTRUCK_LICENSEPLATE");
                strSql.Append(") ;");
            }
            else
            {
                strSql.Append("update DM_LOG_UP_EQUIP set ");
                //strSql.Append(" REQUISITION_CD = :REQUISITION_CD , ");
                //strSql.Append(" JOB_PLAN_CD = :JOB_PLAN_CD , ");
                strSql.Append(" TEAM_ORG_ID = :TEAM_ORG_ID , ");
                strSql.Append(" LOG_SERIES_ID = :LOG_SERIES_ID , ");
                strSql.Append(" WINCH_LICENCEPLATE = :WINCH_LICENCEPLATE , ");
                strSql.Append(" WINCHC_ABLE_LENGTH = :WINCHC_ABLE_LENGTH , ");
                strSql.Append(" LOGGINGTRUCK_LICENSEPLATE = :LOGGINGTRUCK_LICENSEPLATE  ");
                strSql.Append(" where JOB_PLAN_CD=:JOB_PLAN_CD ; ");
            }
            parameters.AddRange(new OracleParameter[] {
			            //ServiceUtils.CreateOracleParameter(":REQUISITION_CD", OracleDbType.Varchar2, model_小队施工地面设备.REQUISITION_CD) ,            
                        //ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, model_小队施工地面设备.JOB_PLAN_CD) ,            
                        ServiceUtils.CreateOracleParameter(":TEAM_ORG_ID", OracleDbType.Varchar2, model_小队施工地面设备.TEAM_ORG_ID) ,            
                        ServiceUtils.CreateOracleParameter(":LOG_SERIES_ID", OracleDbType.Varchar2, model_小队施工地面设备.LOG_SERIES_ID) ,            
                        ServiceUtils.CreateOracleParameter(":WINCH_LICENCEPLATE", OracleDbType.Varchar2, model_小队施工地面设备.WINCH_LICENCEPLATE) ,            
                        ServiceUtils.CreateOracleParameter(":WINCHC_ABLE_LENGTH", OracleDbType.Decimal, model_小队施工地面设备.WINCHC_ABLE_LENGTH) ,            
                        ServiceUtils.CreateOracleParameter(":LOGGINGTRUCK_LICENSEPLATE", OracleDbType.Varchar2, model_小队施工地面设备.LOGGINGTRUCK_LICENSEPLATE)             
              
            });
            strSql.Append(" end;");
            int i = DbHelperOra.ExecuteSqlTran(strSql.ToString(), parameters.ToArray());
            return i > 0 ? true : false;
        }

        [WebMethod(EnableSession = true)]
        public bool Save_测井现场提交信息(string JOB_PLAN_CD, byte[] data_PRO_LOG_RAPID_INFO, byte[] data_PRO_LOG_RAPID_RESULTS, byte[] data_PRO_LOG_RAPID_ORIGINAL_DATA1, byte[] data_PRO_LOG_RAPID_ORIGINAL_DATA)
        {
            Workflow.Controller.ValidateSave<Workflow.C测井现场提交信息>(JOB_PLAN_CD);

            var model_PRO_LOG_RAPID_INFO = Utility.ModelHelper.DeserializeObject(data_PRO_LOG_RAPID_INFO) as Model.PRO_LOG_RAPID_INFO;
            var modelList_PRO_LOG_RAPID_RESULTS = Utility.ModelHelper.DeserializeObject(data_PRO_LOG_RAPID_RESULTS) as List<Model.PRO_LOG_RAPID_RESULTS>;
            var model_PRO_LOG_RAPID_ORIGINAL_DATA = Utility.ModelHelper.DeserializeObject(data_PRO_LOG_RAPID_ORIGINAL_DATA1) as Model.PRO_LOG_RAPID_ORIGINAL_DATA;
            var modelList_PRO_LOG_RAPID_ORIGINAL_DATA = Utility.ModelHelper.DeserializeObject(data_PRO_LOG_RAPID_ORIGINAL_DATA) as List<Model.PRO_LOG_RAPID_ORIGINAL_DATA>;
            if (model_PRO_LOG_RAPID_INFO == null || model_PRO_LOG_RAPID_ORIGINAL_DATA == null) return false;
            StringBuilder strSql = new StringBuilder();
            List<OracleParameter> parameters = new List<OracleParameter>();
            strSql.Append("begin ");
            if (string.IsNullOrWhiteSpace(model_PRO_LOG_RAPID_INFO.JOB_PLAN_CD))
            {
                strSql.Append("insert into PRO_LOG_RAPID_INFO(");
                strSql.Append("REQUISITION_CD,JOB_PLAN_CD,LOG_TEAM,LOG_MODE,LOG_SERVER_ID,BOTTOM_DEP,BOTTOM_TEMPERATURE,MUD_RESITIVITY,MUD_TEMERATURE,SUBMIT_MAN,SUBMIT_DATE,NOTE,BOTTOM_PRESSURE,GROUND_TEMPERATURE");
                strSql.Append(") values (");
                strSql.Append(":REQUISITION_CD,:JOB_PLAN_CD,:LOG_TEAM,:LOG_MODE,:LOG_SERVER_ID,:BOTTOM_DEP,:BOTTOM_TEMPERATURE,:MUD_RESITIVITY,:MUD_TEMERATURE,:SUBMIT_MAN,:SUBMIT_DATE,:NOTE,:BOTTOM_PRESSURE,:GROUND_TEMPERATURE");
                strSql.Append("); ");
            }
            else
            {
                strSql.Append("update PRO_LOG_RAPID_INFO set ");
                //strSql.Append(" REQUISITION_CD = :REQUISITION_CD , ");
                //strSql.Append(" DRILL_JOB_ID = :DRILL_JOB_ID , ");
                //strSql.Append(" JOB_PLAN_CD = :JOB_PLAN_CD , ");
                strSql.Append(" LOG_TEAM = :LOG_TEAM , ");
                strSql.Append(" LOG_MODE = :LOG_MODE , ");
                strSql.Append(" LOG_SERVER_ID = :LOG_SERVER_ID , ");
                strSql.Append(" BOTTOM_DEP = :BOTTOM_DEP , ");
                strSql.Append(" BOTTOM_TEMPERATURE = :BOTTOM_TEMPERATURE , ");
                strSql.Append(" MUD_RESITIVITY = :MUD_RESITIVITY , ");
                strSql.Append(" MUD_TEMERATURE = :MUD_TEMERATURE , ");
                strSql.Append(" SUBMIT_MAN = :SUBMIT_MAN , ");
                strSql.Append(" SUBMIT_DATE = :SUBMIT_DATE , ");
                strSql.Append(" NOTE = :NOTE , ");
                strSql.Append(" BOTTOM_PRESSURE = :BOTTOM_PRESSURE , ");
                strSql.Append(" GROUND_TEMPERATURE = :GROUND_TEMPERATURE  ");
                strSql.Append(" where REQUISITION_CD=:REQUISITION_CD and JOB_PLAN_CD=:JOB_PLAN_CD;");
            }
            parameters.AddRange(new OracleParameter[] {
			            ServiceUtils.CreateOracleParameter(":REQUISITION_CD", OracleDbType.Varchar2, model_PRO_LOG_RAPID_INFO.REQUISITION_CD) ,            
                        //ServiceUtils.CreateOracleParameter(":DRILL_JOB_ID", OracleDbType.Varchar2, model_PRO_LOG_RAPID_INFO.DRILL_JOB_ID) ,            
                        ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2,JOB_PLAN_CD) ,            
                        ServiceUtils.CreateOracleParameter(":LOG_TEAM", OracleDbType.Varchar2, model_PRO_LOG_RAPID_INFO.LOG_TEAM) ,            
                        ServiceUtils.CreateOracleParameter(":LOG_MODE", OracleDbType.Varchar2, model_PRO_LOG_RAPID_INFO.LOG_MODE) ,            
                        ServiceUtils.CreateOracleParameter(":LOG_SERVER_ID", OracleDbType.Varchar2, model_PRO_LOG_RAPID_INFO.LOG_SERVER_ID) ,            
                        ServiceUtils.CreateOracleParameter(":BOTTOM_DEP", OracleDbType.Decimal, model_PRO_LOG_RAPID_INFO.BOTTOM_DEP) ,            
                        ServiceUtils.CreateOracleParameter(":BOTTOM_TEMPERATURE", OracleDbType.Decimal, model_PRO_LOG_RAPID_INFO.BOTTOM_TEMPERATURE) ,            
                        ServiceUtils.CreateOracleParameter(":MUD_RESITIVITY", OracleDbType.Decimal, model_PRO_LOG_RAPID_INFO.MUD_RESITIVITY) ,            
                        ServiceUtils.CreateOracleParameter(":MUD_TEMERATURE", OracleDbType.Decimal, model_PRO_LOG_RAPID_INFO.MUD_TEMERATURE) ,            
                        ServiceUtils.CreateOracleParameter(":SUBMIT_MAN", OracleDbType.Varchar2, model_PRO_LOG_RAPID_INFO.SUBMIT_MAN) ,            
                        ServiceUtils.CreateOracleParameter(":SUBMIT_DATE", OracleDbType.Date, model_PRO_LOG_RAPID_INFO.SUBMIT_DATE) ,            
                        ServiceUtils.CreateOracleParameter(":NOTE", OracleDbType.Varchar2, model_PRO_LOG_RAPID_INFO.NOTE),
                        ServiceUtils.CreateOracleParameter(":BOTTOM_PRESSURE",OracleDbType.Decimal,model_PRO_LOG_RAPID_INFO.BOTTOM_PRESSURE),
                        ServiceUtils.CreateOracleParameter(":GROUND_TEMPERATURE",OracleDbType.Decimal,model_PRO_LOG_RAPID_INFO.GROUND_TEMPERATURE)
            });
            if (modelList_PRO_LOG_RAPID_RESULTS != null && modelList_PRO_LOG_RAPID_RESULTS.Count > 0)
            {
                strSql.Append("delete from PRO_LOG_RAPID_RESULTS where JOB_PLAN_CD=:JOB_PLAN_CD;");
                for (int i = 0; i < modelList_PRO_LOG_RAPID_RESULTS.Count; i++)
                {
                    strSql.Append("insert into PRO_LOG_RAPID_RESULTS(");
                    strSql.Append("DATA_ID,REQUISITION_CD,JOB_PLAN_CD,RAPID_RESULTS_TYPE,START_DEP,END_DEP,DATA_SIZE,NOTE,FILENAME,FILEID");
                    strSql.Append(") values (");
                    strSql.Append("RAPID_RESULTS_DATA_ID_SEQ.NEXTVAL,:REQUISITION_CD,:JOB_PLAN_CD,:RAPID_RESULTS_TYPE" + i + ",:START_DEP" + i + ",:END_DEP" + i + ",:DATA_SIZE" + i + ",:NOTE" + i + ",:FILENAME" + i + ",:FILEID" + i);
                    strSql.Append("); ");
                    parameters.AddRange(new OracleParameter[] {
			            //ServiceUtils.CreateOracleParameter(":DATA_ID", OracleDbType.Varchar2, model.DATA_ID) ,            
                        //ServiceUtils.CreateOracleParameter(":REQUISITION_CD", OracleDbType.Varchar2, model.REQUISITION_CD) ,            
                        //ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, model.JOB_PLAN_CD) ,            
                        ServiceUtils.CreateOracleParameter(":RAPID_RESULTS_TYPE"+i, OracleDbType.Varchar2, modelList_PRO_LOG_RAPID_RESULTS[i].RAPID_RESULTS_TYPE) , 
                        ServiceUtils.CreateOracleParameter(":START_DEP"+i, OracleDbType.Decimal, modelList_PRO_LOG_RAPID_RESULTS[i].START_DEP) ,            
                        ServiceUtils.CreateOracleParameter(":END_DEP"+i, OracleDbType.Decimal, modelList_PRO_LOG_RAPID_RESULTS[i].END_DEP) ,            
                        //ServiceUtils.CreateOracleParameter(":DATA"+i, OracleDbType.Blob, modelList_PRO_LOG_RAPID_RESULTS[i].DATA) ,            
                        ServiceUtils.CreateOracleParameter(":DATA_SIZE"+i, OracleDbType.Decimal, modelList_PRO_LOG_RAPID_RESULTS[i].DATA_SIZE) ,            
                        ServiceUtils.CreateOracleParameter(":NOTE"+i, OracleDbType.Varchar2, modelList_PRO_LOG_RAPID_RESULTS[i].NOTE) ,            
                        ServiceUtils.CreateOracleParameter(":FILENAME"+i, OracleDbType.Varchar2, modelList_PRO_LOG_RAPID_RESULTS[i].FILENAME)   ,
                        ServiceUtils.CreateOracleParameter(":FILEID"+i, OracleDbType.Decimal, modelList_PRO_LOG_RAPID_RESULTS[i].FILEID) 
                    });
                }
            }
            if (modelList_PRO_LOG_RAPID_ORIGINAL_DATA != null && modelList_PRO_LOG_RAPID_ORIGINAL_DATA.Count > 0)
            {
                strSql.Append("delete from PRO_LOG_RAPID_ORIGINAL_DATA where JOB_PLAN_CD=:JOB_PLAN_CD;");
                parameters.AddRange(new OracleParameter[] {
                        ServiceUtils.CreateOracleParameter(":SUBMIT_DATE1", OracleDbType.Date, model_PRO_LOG_RAPID_ORIGINAL_DATA.SUBMIT_DATE) ,            
                        ServiceUtils.CreateOracleParameter(":SUBMIT_MAN1", OracleDbType.Varchar2, model_PRO_LOG_RAPID_ORIGINAL_DATA.SUBMIT_MAN) ,            
                        ServiceUtils.CreateOracleParameter(":DATA_RECIPIENT1", OracleDbType.Varchar2, model_PRO_LOG_RAPID_ORIGINAL_DATA.DATA_RECIPIENT)
                });
                for (int i = 0; i < modelList_PRO_LOG_RAPID_ORIGINAL_DATA.Count; i++)
                {
                    strSql.Append("insert into PRO_LOG_RAPID_ORIGINAL_DATA(");
                    strSql.Append("DATA_ID,JOB_PLAN_CD,DATA_NAME,ORIGINAL_TYPE,START_DEP,END_DEP,LOG_ITEM,DATA_SIZE,SUBMIT_DATE,SUBMIT_MAN,DATA_RECIPIENT,NOTE,FILEID");
                    strSql.Append(") values (");
                    strSql.Append("RAPID_ORIGINAL_DATA_ID_SEQ.NEXTVAL,:JOB_PLAN_CD,:DATA_NAME" + i + ",:ORIGINAL_TYPE" + i + ",:START_DEP0" + i + ",:END_DEP0" + i + ",:LOG_ITEM" + i + ",:DATA_SIZE0" + i + ",:SUBMIT_DATE1,:SUBMIT_MAN1,:DATA_RECIPIENT1,:NOTE0" + i + ",:FILEID0" + i);
                    strSql.Append(");");
                    parameters.AddRange(new OracleParameter[] {
			            //ServiceUtils.CreateOracleParameter(":DATA_ID", OracleDbType.Varchar2, model.DATA_ID) ,            
                        //ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, model.JOB_PLAN_CD) ,            
                        ServiceUtils.CreateOracleParameter(":DATA_NAME"+i, OracleDbType.Varchar2, modelList_PRO_LOG_RAPID_ORIGINAL_DATA[i].DATA_NAME) ,            
                        ServiceUtils.CreateOracleParameter(":ORIGINAL_TYPE"+i, OracleDbType.Varchar2, modelList_PRO_LOG_RAPID_ORIGINAL_DATA[i].ORIGINAL_TYPE) ,            
                        ServiceUtils.CreateOracleParameter(":START_DEP0"+i, OracleDbType.Decimal, modelList_PRO_LOG_RAPID_ORIGINAL_DATA[i].START_DEP) ,            
                        ServiceUtils.CreateOracleParameter(":END_DEP0"+i, OracleDbType.Decimal, modelList_PRO_LOG_RAPID_ORIGINAL_DATA[i].END_DEP) ,            
                        ServiceUtils.CreateOracleParameter(":LOG_ITEM"+i, OracleDbType.Varchar2, modelList_PRO_LOG_RAPID_ORIGINAL_DATA[i].LOG_ITEM) ,            
                        ServiceUtils.CreateOracleParameter(":DATA_SIZE0"+i, OracleDbType.Decimal, modelList_PRO_LOG_RAPID_ORIGINAL_DATA[i].DATA_SIZE) ,                       
                        //ServiceUtils.CreateOracleParameter(":DATA"+i, OracleDbType.Blob, modelList_PRO_LOG_RAPID_ORIGINAL_DATA[i].DATA) ,            
                        ServiceUtils.CreateOracleParameter(":NOTE0"+i, OracleDbType.Varchar2, modelList_PRO_LOG_RAPID_ORIGINAL_DATA[i].NOTE) ,
                        ServiceUtils.CreateOracleParameter(":FILEID0"+i, OracleDbType.Decimal, modelList_PRO_LOG_RAPID_ORIGINAL_DATA[i].FILEID)        
                    });
                }
            }
            strSql.Append(" end;");
            if (DbHelperOra.ExecuteSqlTran(strSql.ToString(), parameters.ToArray()) > 0) return true;
            return false;
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetWorkDetailsFileNames(string JOB_PLAN_CD)
        {
            return DbHelperOra.Query("select Filename from DM_LOG_WORK_DETAILS where JOB_PLAN_CD=:JOB_PLAN_CD and filename is not null",
                ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, JOB_PLAN_CD));
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetData_测井现场提交信息(string JOB_PLAN_CD)
        {
            DataSet ds = new DataSet();
            DataTable dt1 = DbHelperOra.Query("select * from PRO_LOG_RAPID_INFO where job_plan_cd=:JOB_PLAN_CD",
                new OracleParameter[]{
                    ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, JOB_PLAN_CD)
                }).Tables[0];
            DataTable dt2 = DbHelperOra.Query("select * from PRO_LOG_RAPID_RESULTS where job_plan_cd=:JOB_PLAN_CD",
                new OracleParameter[]{
                    ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, JOB_PLAN_CD)
                }).Tables[0];

            DataTable dt3 = DbHelperOra.Query("select * from PRO_LOG_RAPID_ORIGINAL_DATA where job_plan_cd=:JOB_PLAN_CD",
                new OracleParameter[]{
                    ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, JOB_PLAN_CD)
                }).Tables[0];

            dt1.TableName = "PRO_LOG_RAPID_INFO";
            dt2.TableName = "PRO_LOG_RAPID_RESULTS";
            dt3.TableName = "PRO_LOG_RAPID_ORIGINAL_DATA";

            ds.Tables.Add(dt1.Copy());
            ds.Tables.Add(dt2.Copy());
            ds.Tables.Add(dt3.Copy());
            return ds;
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetData_小队施工基本信息(string JOB_PLAN_CD)
        {
            if (string.IsNullOrWhiteSpace(JOB_PLAN_CD)) return null;
            DataSet ds = new DataSet();
            DataTable dt1 = DbHelperOra.Query("select a.*,b.drill_statuse from dm_log_base a,dm_log_task b,DM_LOG_OPS_PLAN c where a.job_plan_cd=:JOB_PLAN_CD and a.job_plan_cd=c.job_plan_cd and b.requisition_cd=c.requisition_cd", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, JOB_PLAN_CD) }).Tables[0];
            DataTable dt2 = DbHelperOra.Query("select * from dm_log_work_aning where job_plan_cd=:JOB_PLAN_CD", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, JOB_PLAN_CD) }).Tables[0];
            DataTable dt3 = DbHelperOra.Query("select * from dm_log_work_personnel where job_plan_cd=:JOB_PLAN_CD", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, JOB_PLAN_CD) }).Tables[0];
            DataTable dt4 = DbHelperOra.Query("select * from dm_log_up_equip where job_plan_cd=:JOB_PLAN_CD", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, JOB_PLAN_CD) }).Tables[0];
            dt1.TableName = "DM_LOG_BASE";
            dt2.TableName = "DM_LOG_WORK_ANING";
            dt3.TableName = "DM_LOG_WORK_PERSONNEL";
            dt4.TableName = "DM_LOG_UP_EQUIP";
            ds.Tables.Add(dt1.Copy());
            ds.Tables.Add(dt2.Copy());
            ds.Tables.Add(dt3.Copy());
            ds.Tables.Add(dt4.Copy());
            return ds;
        }

        [WebMethod(EnableSession = true)]
        public bool Save_多方联席会(byte[] data_多方联席会)
        {
            if (data_多方联席会 == null) return false;
            Model.DM_LOG_CONTACT_WILL model = Utility.ModelHelper.DeserializeObject(data_多方联席会) as Model.DM_LOG_CONTACT_WILL;

            Workflow.Controller.ValidateSave<Workflow.C测井现场提交信息>(model.JOB_PLAN_CD);

            StringBuilder strSql = new StringBuilder();
            List<OracleParameter> parameters = new List<OracleParameter>();
            if (string.IsNullOrWhiteSpace(model.CONTACTID))
            {
                strSql.Append("insert into DM_LOG_CONTACT_WILL(");
                strSql.Append("CONTACTID,JOB_PLAN_CD,A_PUNISH,RRSIDE_SITE_PUNISH,DRILL_CREW_CHIEF,LOG_TEAM_LEADER,LOG_OPERATOR__NAME,LOG_SAFE_NAME,LOG_LAND_LEADER,LOG_COMPANY_PUNISH,LOG_COMPANY_LEADER,MEETING_SUM,WRITER,MEETING_DATE,MEETING_TIME,NOTE,B_PUNISH,MUD_CREW_CHIEF,SOLP_CREW_CHIEF,LOG_INTERPRETATION");
                strSql.Append(") values (");
                strSql.Append("CONTACTID_SEQUENCE.nextval,:JOB_PLAN_CD,:A_PUNISH,:RRSIDE_SITE_PUNISH,:DRILL_CREW_CHIEF,:LOG_TEAM_LEADER,:LOG_OPERATOR__NAME,:LOG_SAFE_NAME,:LOG_LAND_LEADER,:LOG_COMPANY_PUNISH,:LOG_COMPANY_LEADER,:MEETING_SUM,:WRITER,:MEETING_DATE,:MEETING_TIME,:NOTE,:B_PUNISH,:MUD_CREW_CHIEF,:SOLP_CREW_CHIEF,:LOG_INTERPRETATION");
                strSql.Append(") ");
                parameters.Add(ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, model.JOB_PLAN_CD));
            }
            else
            {
                strSql.Append("update DM_LOG_CONTACT_WILL set ");
                //strSql.Append(" CONTACTID = :CONTACTID , ");
                //strSql.Append(" JOB_PLAN_CD = :JOB_PLAN_CD , ");
                strSql.Append(" A_PUNISH = :A_PUNISH , ");
                strSql.Append(" RRSIDE_SITE_PUNISH = :RRSIDE_SITE_PUNISH , ");
                strSql.Append(" DRILL_CREW_CHIEF = :DRILL_CREW_CHIEF , ");
                strSql.Append(" LOG_TEAM_LEADER = :LOG_TEAM_LEADER , ");
                strSql.Append(" LOG_OPERATOR__NAME = :LOG_OPERATOR__NAME , ");
                strSql.Append(" LOG_SAFE_NAME = :LOG_SAFE_NAME , ");
                strSql.Append(" LOG_LAND_LEADER = :LOG_LAND_LEADER , ");
                strSql.Append(" LOG_COMPANY_PUNISH = :LOG_COMPANY_PUNISH , ");
                strSql.Append(" LOG_COMPANY_LEADER = :LOG_COMPANY_LEADER , ");
                strSql.Append(" MEETING_SUM = :MEETING_SUM , ");
                strSql.Append(" WRITER = :WRITER , ");
                strSql.Append(" MEETING_DATE = :MEETING_DATE , ");
                strSql.Append(" MEETING_TIME = :MEETING_TIME , ");
                strSql.Append(" NOTE = :NOTE , ");
                strSql.Append(" B_PUNISH = :B_PUNISH , ");
                strSql.Append(" MUD_CREW_CHIEF = :MUD_CREW_CHIEF , ");
                strSql.Append(" SOLP_CREW_CHIEF = :SOLP_CREW_CHIEF , ");
                strSql.Append(" LOG_INTERPRETATION = :LOG_INTERPRETATION  ");
                strSql.Append(" where CONTACTID=:CONTACTID  ");
                parameters.Add(ServiceUtils.CreateOracleParameter(":CONTACTID", OracleDbType.Char, model.CONTACTID));
            }
            parameters.AddRange(new OracleParameter[] {          
                        ServiceUtils.CreateOracleParameter(":A_PUNISH", OracleDbType.Char, model.A_PUNISH) ,            
                        ServiceUtils.CreateOracleParameter(":RRSIDE_SITE_PUNISH", OracleDbType.Char, model.RRSIDE_SITE_PUNISH) ,            
                        ServiceUtils.CreateOracleParameter(":DRILL_CREW_CHIEF", OracleDbType.Char, model.DRILL_CREW_CHIEF) ,            
                        ServiceUtils.CreateOracleParameter(":LOG_TEAM_LEADER", OracleDbType.Char, model.LOG_TEAM_LEADER) ,            
                        ServiceUtils.CreateOracleParameter(":LOG_OPERATOR__NAME", OracleDbType.Varchar2, model.LOG_OPERATOR__NAME) ,            
                        ServiceUtils.CreateOracleParameter(":LOG_SAFE_NAME", OracleDbType.Char, model.LOG_SAFE_NAME) ,            
                        ServiceUtils.CreateOracleParameter(":LOG_LAND_LEADER", OracleDbType.Char, model.LOG_LAND_LEADER) ,            
                        ServiceUtils.CreateOracleParameter(":LOG_COMPANY_PUNISH", OracleDbType.Char, model.LOG_COMPANY_PUNISH) ,            
                        ServiceUtils.CreateOracleParameter(":LOG_COMPANY_LEADER", OracleDbType.Char, model.LOG_COMPANY_LEADER) ,            
                        ServiceUtils.CreateOracleParameter(":MEETING_SUM", OracleDbType.Char, model.MEETING_SUM) ,            
                        ServiceUtils.CreateOracleParameter(":WRITER", OracleDbType.Char, model.WRITER) ,            
                        ServiceUtils.CreateOracleParameter(":MEETING_DATE", OracleDbType.Date, model.MEETING_DATE) ,            
                        ServiceUtils.CreateOracleParameter(":MEETING_TIME", OracleDbType.Decimal, model.MEETING_TIME) ,            
                        ServiceUtils.CreateOracleParameter(":NOTE", OracleDbType.Varchar2, model.NOTE) ,            
                        ServiceUtils.CreateOracleParameter(":B_PUNISH", OracleDbType.Char, model.B_PUNISH) ,            
                        ServiceUtils.CreateOracleParameter(":MUD_CREW_CHIEF", OracleDbType.Char, model.MUD_CREW_CHIEF) ,            
                        ServiceUtils.CreateOracleParameter(":SOLP_CREW_CHIEF", OracleDbType.Char, model.SOLP_CREW_CHIEF) ,            
                        ServiceUtils.CreateOracleParameter(":LOG_INTERPRETATION", OracleDbType.Char, model.LOG_INTERPRETATION)
            });

            if (DbHelperOra.ExecuteSqlTran(strSql.ToString(), parameters.ToArray()) > 0) return true;
            return false;
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetData_多方联席会(string JOB_PLAN_CD)
        {
            if (string.IsNullOrWhiteSpace(JOB_PLAN_CD)) return null;
            return DbHelperOra.Query("select * from DM_LOG_CONTACT_WILL where job_plan_cd=:JOB_PLAN_CD",
                new OracleParameter[] { ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, JOB_PLAN_CD) });
        }
        [WebMethod(EnableSession = true)]
        public DataSet GetDataList_项目明细(string JOB_PLAN_CD)
        {
            return DbHelperOra.Query("select job_plan_cd,down_well_sequence,COMBINATION_NAME,LOG_SERIES_NAME,START_TIME,END_TIME,WORK_HOURS,measure_well_to,measure_well_from,decode(if_success,'1','是','0','否','') as if_success,note from dm_log_work_details where job_plan_cd=:JOB_PLAN_CD order by down_well_sequence",
                new OracleParameter[] { ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, JOB_PLAN_CD) });
        }
        [WebMethod(EnableSession = true)]
        public DataSet GetDataList_井下仪器编码()
        {
            return DbHelperOra.Query("select instrument_id,instrument_name,instrument_type,instrument_model,instrument_zbh,use_team from pkl_log_downhole_instrument order by instrument_name");
        }
        [WebMethod(EnableSession = true)]
        public DataSet GetDataList_车辆信息()
        {
            return DbHelperOra.Query("select vehicle_id,vehicle_type,vehicle_plate,engine_power,nuclear_load_man1,team_org,initial_date from pkl_log_vehicle order by  initial_date desc");
        }
        [WebMethod(EnableSession = true)]
        public DataSet GetData_测井任务通知单(string string_通知单编码)
        {
            return DbHelperOra.Query("select * from dm_log_task where requisition_cd=:REQUISITION_CD",
                new OracleParameter[] { ServiceUtils.CreateOracleParameter(":REQUISITION_CD", OracleDbType.Varchar2, string_通知单编码) });
        }
        [WebMethod(EnableSession = true)]
        public DataSet GetDataList_放射源编码()
        {
            return DbHelperOra.Query("select RADIATION_NAME,RADIATION_NO,RADIATION_ID,ELEMENT,ACTIVE,USE_TEAM from pkl_log_radiation order by RADIATION_NAME");
        }

        [WebMethod(EnableSession = true)]
        public bool Save_作业明细(byte[] data_作业明细, byte[] data_遇阻情况, byte[] data_放射源使用情况, byte[] dataChangedList_测井项目, byte[] dataRemoveList_测井项目, byte[] dataChangedList_井下设备, byte[] dataRemoveList_井下设备)
        {
            if (data_作业明细 == null || data_遇阻情况 == null || data_放射源使用情况 == null) return false;
            var model_作业明细 = Utility.ModelHelper.DeserializeObject(data_作业明细) as Model.DM_LOG_WORK_DETAILS;
            var model_遇阻情况 = Utility.ModelHelper.DeserializeObject(data_遇阻情况) as Model.DM_LOG_WORK_HOLDUP_DETAILS;
            var model_放射源使用情况 = Utility.ModelHelper.DeserializeObject(data_放射源使用情况) as Model.DM_LOG_RADIATION_STATUS;
            if (model_作业明细.DOWN_WELL_SEQUENCE == null || model_作业明细.DOWN_WELL_SEQUENCE < 1) ServiceUtils.ThrowSoapException("下井趟次号不正确！");

            Workflow.Controller.ValidateSave<Workflow.C测井现场提交信息>(model_作业明细.JOB_PLAN_CD);

            StringBuilder strSql = new StringBuilder();
            List<OracleParameter> parameters = new List<OracleParameter>();
            strSql.Append("begin ");
            parameters.Add(ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, model_作业明细.JOB_PLAN_CD));
            parameters.Add(ServiceUtils.CreateOracleParameter(":DOWN_WELL_SEQUENCE", OracleDbType.Decimal, model_作业明细.DOWN_WELL_SEQUENCE));
            if (string.IsNullOrWhiteSpace(model_遇阻情况.JOB_PLAN_CD))
            {
                if (DbHelperOra.Exists("select 1 from dm_log_work_details where job_plan_cd=:JOB_PLAN_CD and down_well_sequence=" + model_作业明细.DOWN_WELL_SEQUENCE,
                    new OracleParameter[] { ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, model_作业明细.JOB_PLAN_CD) }))
                {
                    ServiceUtils.ThrowSoapException("下井趟次号已经存在！");
                }
                strSql.Append("insert into DM_LOG_WORK_DETAILS(");
                strSql.Append("DETAILSID,JOB_PLAN_CD,BLOCK_NUNBER,LOG_ORIGIANL_DATA_ID,DOWN_WELL_SEQUENCE,IS_ADD,COMBINATION_NAME,START_TIME,END_TIME,WORK_HOURS,IF_SUCCESS,MEASURE_WELL_TO,MEASURE_WELL_FROM,WELL_SECTION,POWERSUPPLY_VOLTAGE,CABLEHEAD_VOLTAGE,\"CURRENT\",LARGEST_CABLE_TENSION,LARGEST_CABLEHEAD_TENSION,DATA_NAME,DATA_SIZE,DATA_BLOCK_NUM,NOTE,FILENAME,LOG_SERIES_NAME");
                strSql.Append(") values (");
                strSql.Append("DETAILSID_SEQUENCE.nextval,:JOB_PLAN_CD,:BLOCK_NUNBER,:LOG_ORIGIANL_DATA_ID,:DOWN_WELL_SEQUENCE,:IS_ADD,:COMBINATION_NAME,:START_TIME,:END_TIME,:WORK_HOURS,:IF_SUCCESS,:MEASURE_WELL_TO,:MEASURE_WELL_FROM,:WELL_SECTION,:POWERSUPPLY_VOLTAGE,:CABLEHEAD_VOLTAGE,:CURRENT,:LARGEST_CABLE_TENSION,:LARGEST_CABLEHEAD_TENSION,:DATA_NAME,:DATA_SIZE,:DATA_BLOCK_NUM,:NOTE,:FILENAME,:LOG_SERIES_NAME");
                strSql.Append("); ");

                strSql.Append("insert into DM_LOG_WORK_HOLDUP_DETAILS(");
                strSql.Append("HOLDUPID,JOB_PLAN_CD,DOWN_WELL_SEQUENCE,HOLDUP_DATE,HOLDUP_MD,HOLDUP_TYPE,WB_OBSTRUCTION_ID,OBSTRUCTION_DESC,TOOL_DIAMETER");
                strSql.Append(") values (");
                strSql.Append("HOLDUPID_SEQUENCE.nextval,:JOB_PLAN_CD,:DOWN_WELL_SEQUENCE,:HOLDUP_DATE,:HOLDUP_MD,:HOLDUP_TYPE,:WB_OBSTRUCTION_ID,:OBSTRUCTION_DESC,:TOOL_DIAMETER");
                strSql.Append("); ");

                strSql.Append("insert into DM_LOG_RADIATION_STATUS(");
                strSql.Append("JOB_PLAN_CD,RADIATION_NO,RADIATION_CD,ELEMENT,ACTIVE,LOAD_PERSON,UNLOAD_PERSON,UNDER_WELL_TIME,MAX_WORK_PRESSURE,MAX_WORK_TEMPERATURE,REPLACE_SOURCEPACKING,REPLACE_SOURCEPACKING_NUM,REPLACE_DATE,REPLACE_PLACE,REPLACE_PERSON,RADIATIONID,DOWN_WELL_SEQUENCE");
                strSql.Append(") values (");
                strSql.Append(":JOB_PLAN_CD,:RADIATION_NO,:RADIATION_CD,:ELEMENT,:ACTIVE,:LOAD_PERSON,:UNLOAD_PERSON,:UNDER_WELL_TIME,:MAX_WORK_PRESSURE,:MAX_WORK_TEMPERATURE,:REPLACE_SOURCEPACKING,:REPLACE_SOURCEPACKING_NUM,:REPLACE_DATE,:REPLACE_PLACE,:REPLACE_PERSON,:RADIATIONID,:DOWN_WELL_SEQUENCE");
                strSql.Append("); ");
            }
            else
            {
                strSql.Append("update DM_LOG_WORK_DETAILS set ");
                //strSql.Append(" DETAILSID = :DETAILSID , ");
                //strSql.Append(" JOB_PLAN_CD = :JOB_PLAN_CD , ");
                strSql.Append(" BLOCK_NUNBER = :BLOCK_NUNBER , ");
                strSql.Append(" LOG_ORIGIANL_DATA_ID = :LOG_ORIGIANL_DATA_ID , ");
                //strSql.Append(" DOWN_WELL_SEQUENCE = :DOWN_WELL_SEQUENCE , ");
                strSql.Append(" IS_ADD = :IS_ADD , ");
                strSql.Append(" COMBINATION_NAME = :COMBINATION_NAME , ");
                strSql.Append(" START_TIME = :START_TIME , ");
                strSql.Append(" END_TIME = :END_TIME , ");
                strSql.Append(" WORK_HOURS = :WORK_HOURS , ");
                strSql.Append(" IF_SUCCESS = :IF_SUCCESS , ");
                strSql.Append(" MEASURE_WELL_TO = :MEASURE_WELL_TO , ");
                strSql.Append(" MEASURE_WELL_FROM = :MEASURE_WELL_FROM , ");
                strSql.Append(" WELL_SECTION = :WELL_SECTION , ");
                strSql.Append(" POWERSUPPLY_VOLTAGE = :POWERSUPPLY_VOLTAGE , ");
                strSql.Append(" CABLEHEAD_VOLTAGE = :CABLEHEAD_VOLTAGE , ");
                strSql.Append(" \"CURRENT\" = :CURRENT , ");
                strSql.Append(" LARGEST_CABLE_TENSION = :LARGEST_CABLE_TENSION , ");
                strSql.Append(" LARGEST_CABLEHEAD_TENSION = :LARGEST_CABLEHEAD_TENSION , ");
                strSql.Append(" DATA_NAME = :DATA_NAME , ");
                strSql.Append(" DATA_SIZE = :DATA_SIZE , ");
                strSql.Append(" DATA_BLOCK_NUM = :DATA_BLOCK_NUM , ");
                strSql.Append(" NOTE = :NOTE , ");
                strSql.Append(" FILENAME = :FILENAME ,");
                strSql.Append(" LOG_SERIES_NAME=:LOG_SERIES_NAME ");
                strSql.Append(" where DETAILSID=:DETAILSID;");
                parameters.Add(ServiceUtils.CreateOracleParameter(":DETAILSID", OracleDbType.Varchar2, model_作业明细.DETAILSID));

                strSql.Append("update DM_LOG_WORK_HOLDUP_DETAILS set ");
                //strSql.Append(" HOLDUPID = :HOLDUPID , ");
                //strSql.Append(" JOB_PLAN_CD = :JOB_PLAN_CD , ");
                //strSql.Append(" DOWN_WELL_SEQUENCE = :DOWN_WELL_SEQUENCE , ");
                strSql.Append(" HOLDUP_DATE = :HOLDUP_DATE , ");
                strSql.Append(" HOLDUP_MD = :HOLDUP_MD , ");
                strSql.Append(" HOLDUP_TYPE = :HOLDUP_TYPE , ");
                strSql.Append(" WB_OBSTRUCTION_ID = :WB_OBSTRUCTION_ID , ");
                strSql.Append(" OBSTRUCTION_DESC = :OBSTRUCTION_DESC , ");
                strSql.Append(" TOOL_DIAMETER = :TOOL_DIAMETER  ");
                strSql.Append(" where HOLDUPID=:HOLDUPID;");
                parameters.Add(ServiceUtils.CreateOracleParameter(":HOLDUPID", OracleDbType.Varchar2, model_遇阻情况.HOLDUPID));

                strSql.Append("update DM_LOG_RADIATION_STATUS set ");
                //strSql.Append(" JOB_PLAN_CD = :JOB_PLAN_CD , ");
                strSql.Append(" RADIATION_NO = :RADIATION_NO , ");
                strSql.Append(" RADIATION_CD = :RADIATION_CD , ");
                strSql.Append(" ELEMENT = :ELEMENT , ");
                strSql.Append(" ACTIVE = :ACTIVE , ");
                strSql.Append(" LOAD_PERSON = :LOAD_PERSON , ");
                strSql.Append(" UNLOAD_PERSON = :UNLOAD_PERSON , ");
                strSql.Append(" UNDER_WELL_TIME = :UNDER_WELL_TIME , ");
                strSql.Append(" MAX_WORK_PRESSURE = :MAX_WORK_PRESSURE , ");
                strSql.Append(" MAX_WORK_TEMPERATURE = :MAX_WORK_TEMPERATURE , ");
                strSql.Append(" REPLACE_SOURCEPACKING = :REPLACE_SOURCEPACKING , ");
                strSql.Append(" REPLACE_SOURCEPACKING_NUM = :REPLACE_SOURCEPACKING_NUM , ");
                strSql.Append(" REPLACE_DATE = :REPLACE_DATE , ");
                strSql.Append(" REPLACE_PLACE = :REPLACE_PLACE , ");
                strSql.Append(" REPLACE_PERSON = :REPLACE_PERSON , ");
                strSql.Append(" RADIATIONID = :RADIATIONID ");
                // strSql.Append(" DOWN_WELL_SEQUENCE = :DOWN_WELL_SEQUENCE  ");
                strSql.Append(" where JOB_PLAN_CD=:JOB_PLAN_CD and DOWN_WELL_SEQUENCE =:DOWN_WELL_SEQUENCE ; ");
            }
            parameters.AddRange(new OracleParameter[] {
                        ServiceUtils.CreateOracleParameter(":BLOCK_NUNBER", OracleDbType.Decimal, model_作业明细.BLOCK_NUNBER) ,         
                        ServiceUtils.CreateOracleParameter(":LOG_ORIGIANL_DATA_ID", OracleDbType.Varchar2, model_作业明细.LOG_ORIGIANL_DATA_ID) ,            
                        //ServiceUtils.CreateOracleParameter(":DOWN_WELL_SEQUENCE", OracleDbType.Decimal, model_作业明细.DOWN_WELL_SEQUENCE) ,            
                        ServiceUtils.CreateOracleParameter(":IS_ADD", OracleDbType.Char, model_作业明细.IS_ADD) ,            
                        ServiceUtils.CreateOracleParameter(":COMBINATION_NAME", OracleDbType.Varchar2, model_作业明细.COMBINATION_NAME) ,
                        ServiceUtils.CreateOracleParameter(":START_TIME", OracleDbType.Date, model_作业明细.START_TIME) ,           
                        ServiceUtils.CreateOracleParameter(":END_TIME", OracleDbType.Date, model_作业明细.END_TIME) ,            
                        ServiceUtils.CreateOracleParameter(":WORK_HOURS", OracleDbType.Decimal, model_作业明细.WORK_HOURS) ,            
                        ServiceUtils.CreateOracleParameter(":IF_SUCCESS", OracleDbType.Char, model_作业明细.IF_SUCCESS) ,            
                        ServiceUtils.CreateOracleParameter(":MEASURE_WELL_TO", OracleDbType.Decimal, model_作业明细.MEASURE_WELL_TO) ,   
                        ServiceUtils.CreateOracleParameter(":MEASURE_WELL_FROM", OracleDbType.Decimal, model_作业明细.MEASURE_WELL_FROM) ,            
                        ServiceUtils.CreateOracleParameter(":WELL_SECTION", OracleDbType.Varchar2, model_作业明细.WELL_SECTION) ,        
                        ServiceUtils.CreateOracleParameter(":POWERSUPPLY_VOLTAGE", OracleDbType.Decimal, model_作业明细.POWERSUPPLY_VOLTAGE) ,            
                        ServiceUtils.CreateOracleParameter(":CABLEHEAD_VOLTAGE", OracleDbType.Decimal, model_作业明细.CABLEHEAD_VOLTAGE) ,            
                        ServiceUtils.CreateOracleParameter(":CURRENT", OracleDbType.Decimal, model_作业明细.CURRENT) ,            
                        ServiceUtils.CreateOracleParameter(":LARGEST_CABLE_TENSION", OracleDbType.Decimal, model_作业明细.LARGEST_CABLE_TENSION) ,            
                        ServiceUtils.CreateOracleParameter(":LARGEST_CABLEHEAD_TENSION", OracleDbType.Decimal, model_作业明细.LARGEST_CABLEHEAD_TENSION) ,            
                        ServiceUtils.CreateOracleParameter(":DATA_NAME", OracleDbType.Varchar2, model_作业明细.DATA_NAME) ,            
                        ServiceUtils.CreateOracleParameter(":DATA_SIZE", OracleDbType.Decimal, model_作业明细.DATA_SIZE) ,            
                        ServiceUtils.CreateOracleParameter(":DATA_BLOCK_NUM", OracleDbType.Decimal, model_作业明细.DATA_BLOCK_NUM) ,     
                        ServiceUtils.CreateOracleParameter(":NOTE", OracleDbType.Varchar2, model_作业明细.NOTE) ,            
                        ServiceUtils.CreateOracleParameter(":FILENAME", OracleDbType.Clob, model_作业明细.FILENAME),
                        ServiceUtils.CreateOracleParameter(":LOG_SERIES_NAME", OracleDbType.Varchar2, model_作业明细.LOG_SERIES_NAME)  
  
                });
            parameters.AddRange(new OracleParameter[] {       
                        //ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, model_遇阻情况.JOB_PLAN_CD) ,       
                       // ServiceUtils.CreateOracleParameter(":DOWN_WELL_SEQUENCE", OracleDbType.Decimal, model_遇阻情况.DOWN_WELL_SEQUENCE) ,            
                        ServiceUtils.CreateOracleParameter(":HOLDUP_DATE", OracleDbType.Date, model_遇阻情况.HOLDUP_DATE) ,         
                        ServiceUtils.CreateOracleParameter(":HOLDUP_MD", OracleDbType.Varchar2, model_遇阻情况.HOLDUP_MD) ,            
                        ServiceUtils.CreateOracleParameter(":HOLDUP_TYPE", OracleDbType.Varchar2, model_遇阻情况.HOLDUP_TYPE) ,          
                        ServiceUtils.CreateOracleParameter(":WB_OBSTRUCTION_ID", OracleDbType.Varchar2, model_遇阻情况.WB_OBSTRUCTION_ID) ,            
                        ServiceUtils.CreateOracleParameter(":OBSTRUCTION_DESC", OracleDbType.Varchar2, model_遇阻情况.OBSTRUCTION_DESC) , 
                        ServiceUtils.CreateOracleParameter(":TOOL_DIAMETER", OracleDbType.Decimal, model_遇阻情况.TOOL_DIAMETER)         
            });
            parameters.AddRange(new OracleParameter[] {
			            //ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, model_放射源使用情况.JOB_PLAN_CD) ,            
                        ServiceUtils.CreateOracleParameter(":RADIATION_NO", OracleDbType.Varchar2, model_放射源使用情况.RADIATION_NO) ,            
                        ServiceUtils.CreateOracleParameter(":RADIATION_CD", OracleDbType.Varchar2, model_放射源使用情况.RADIATION_CD) ,            
                        ServiceUtils.CreateOracleParameter(":ELEMENT", OracleDbType.Varchar2, model_放射源使用情况.ELEMENT) ,            
                        ServiceUtils.CreateOracleParameter(":ACTIVE", OracleDbType.Varchar2, model_放射源使用情况.ACTIVE) ,            
                        ServiceUtils.CreateOracleParameter(":LOAD_PERSON", OracleDbType.Varchar2, model_放射源使用情况.LOAD_PERSON) ,            
                        ServiceUtils.CreateOracleParameter(":UNLOAD_PERSON", OracleDbType.Varchar2, model_放射源使用情况.UNLOAD_PERSON) ,            
                        ServiceUtils.CreateOracleParameter(":UNDER_WELL_TIME", OracleDbType.Decimal, model_放射源使用情况.UNDER_WELL_TIME) ,           
                        ServiceUtils.CreateOracleParameter(":MAX_WORK_PRESSURE", OracleDbType.Decimal, model_放射源使用情况.MAX_WORK_PRESSURE) ,       
                        ServiceUtils.CreateOracleParameter(":MAX_WORK_TEMPERATURE", OracleDbType.Decimal, model_放射源使用情况.MAX_WORK_TEMPERATURE) , 
                        ServiceUtils.CreateOracleParameter(":REPLACE_SOURCEPACKING", OracleDbType.Varchar2, model_放射源使用情况.REPLACE_SOURCEPACKING) ,            
                        ServiceUtils.CreateOracleParameter(":REPLACE_SOURCEPACKING_NUM", OracleDbType.Decimal, model_放射源使用情况.REPLACE_SOURCEPACKING_NUM) ,            
                        ServiceUtils.CreateOracleParameter(":REPLACE_DATE", OracleDbType.Date, model_放射源使用情况.REPLACE_DATE) , 
                        ServiceUtils.CreateOracleParameter(":REPLACE_PLACE", OracleDbType.Varchar2, model_放射源使用情况.REPLACE_PLACE) ,
                        ServiceUtils.CreateOracleParameter(":REPLACE_PERSON", OracleDbType.Varchar2, model_放射源使用情况.REPLACE_PERSON) ,            
                        ServiceUtils.CreateOracleParameter(":RADIATIONID", OracleDbType.Varchar2, model_放射源使用情况.RADIATIONID)      
                       // ServiceUtils.CreateOracleParameter(":DOWN_WELL_SEQUENCE", OracleDbType.Decimal, model.DOWN_WELL_SEQUENCE)     
            });
            if (dataChangedList_测井项目 != null)
            {
                var model1 = Utility.ModelHelper.DeserializeObject(dataChangedList_测井项目) as List<Model.DM_LOG_ITEMS>;
                if (model1 != null)
                {
                    for (int i = 0; i < model1.Count; ++i)
                    {
                        if (model1[i].DOWN_WELL_SEQUENCE == null)
                        {
                            strSql.Append("insert into DM_LOG_ITEMS(");
                            strSql.Append("JOB_PLAN_CD,DOWN_WELL_SEQUENCE,LOGGING_NAME,ITEM_FLAG,ST_DEP,EN_DEP,SCALE,RLEV,NOTE,LOG_ITEM_ID");
                            strSql.Append(") values (");
                            strSql.Append(":JOB_PLAN_CD,:DOWN_WELL_SEQUENCE,:LOGGING_NAME" + i + ",:ITEM_FLAG" + i + ",:ST_DEP" + i + ",:EN_DEP" + i + ",:SCALE" + i + ",:RLEV" + i + ",:NOTE" + i + "," + model1[i].LOG_ITEM_ID);
                            strSql.Append("); ");
                        }
                        else
                        {
                            strSql.Append("update DM_LOG_ITEMS set ");
                            //strSql.Append(" JOB_PLAN_CD = :JOB_PLAN_CD , ");
                            //strSql.Append(" DOWN_WELL_SEQUENCE = :DOWN_WELL_SEQUENCE , ");
                            strSql.Append(" LOGGING_NAME = :LOGGING_NAME" + i + " , ");
                            strSql.Append(" ITEM_FLAG = :ITEM_FLAG" + i + " , ");
                            strSql.Append(" ST_DEP = :ST_DEP" + i + " , ");
                            strSql.Append(" EN_DEP = :EN_DEP" + i + " , ");
                            strSql.Append(" SCALE = :SCALE" + i + " , ");
                            strSql.Append(" RLEV = :RLEV" + i + " , ");
                            strSql.Append(" NOTE = :NOTE" + i);
                            //strSql.Append(" LOG_ITEM_ID = :LOG_ITEM_ID ");
                            strSql.Append(" where JOB_PLAN_CD=:JOB_PLAN_CD and DOWN_WELL_SEQUENCE = :DOWN_WELL_SEQUENCE and LOG_ITEM_ID=" + model1[i].LOG_ITEM_ID + ";");
                        }
                        parameters.AddRange(new OracleParameter[] {
			            //ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, model.JOB_PLAN_CD) ,            
                        //ServiceUtils.CreateOracleParameter(":DOWN_WELL_SEQUENCE", OracleDbType.Decimal, model.DOWN_WELL_SEQUENCE) ,   
                        ServiceUtils.CreateOracleParameter(":LOGGING_NAME" + i, OracleDbType.Varchar2, model1[i].LOGGING_NAME) ,  
                        ServiceUtils.CreateOracleParameter(":ITEM_FLAG" + i, OracleDbType.Decimal, model1[i].ITEM_FLAG) ,            
                        ServiceUtils.CreateOracleParameter(":ST_DEP" + i, OracleDbType.Decimal, model1[i].ST_DEP) ,            
                        ServiceUtils.CreateOracleParameter(":EN_DEP" + i, OracleDbType.Decimal, model1[i].EN_DEP) ,            
                        ServiceUtils.CreateOracleParameter(":SCALE" + i, OracleDbType.Varchar2, model1[i].SCALE) ,            
                        ServiceUtils.CreateOracleParameter(":RLEV" + i, OracleDbType.Decimal, model1[i].RLEV) ,            
                        ServiceUtils.CreateOracleParameter(":NOTE" + i, OracleDbType.NVarchar2, model1[i].NOTE)});
                        //ServiceUtils.CreateOracleParameter(":LOG_ITEM_ID" + i, OracleDbType.Decimal, model1[i].LOG_ITEM_ID)
                    }
                }
                if (dataRemoveList_测井项目 != null)
                {
                    var model2 = Utility.ModelHelper.DeserializeObject(dataRemoveList_测井项目) as List<Model.DM_LOG_ITEMS>;
                    if (model2 != null)
                    {
                        for (int i = 0; i < model2.Count; ++i)
                        {
                            strSql.Append("delete from DM_LOG_ITEMS ");
                            strSql.Append(" where JOB_PLAN_CD=:JOB_PLAN_CD and DOWN_WELL_SEQUENCE = :DOWN_WELL_SEQUENCE and LOG_ITEM_ID=" + model2[i].LOG_ITEM_ID + ";");
                        }
                    }
                }
            }
            if (dataChangedList_井下设备 != null)
            {
                var model3 = Utility.ModelHelper.DeserializeObject(dataChangedList_井下设备) as List<Model.DM_LOG_DOWNHOLE_EQUIP>;
                if (model3 != null)
                {
                    for (int i = 0; i < model3.Count; ++i)
                    {
                        if (model3[i].DOWN_WELL_SEQUENCE == null)
                        {
                            strSql.Append("insert into DM_LOG_DOWNHOLE_EQUIP(");
                            strSql.Append("JOB_PLAN_CD,EQUIPID,DEVICEACCOUNT_NAME,DEVICEACCOUNT_ID,TEAM,WORKING_STATE,FAULT_DESCRIPTION,DOWN_WELL_SEQUENCE");
                            strSql.Append(") values (");
                            strSql.Append(":JOB_PLAN_CD,EQUIPID_SEQUENCE.nextval,:DEVICEACCOUNT_NAME" + i + ",:DEVICEACCOUNT_ID" + i + ",:TEAM" + i + ",:WORKING_STATE" + i + ",:FAULT_DESCRIPTION" + i + ",:DOWN_WELL_SEQUENCE");
                            strSql.Append(") ;");
                        }
                        else
                        {
                            strSql.Append("update DM_LOG_DOWNHOLE_EQUIP set ");
                            //strSql.Append(" JOB_PLAN_CD = :JOB_PLAN_CD , ");
                            //strSql.Append(" EQUIPID = :EQUIPID , ");
                            strSql.Append(" DEVICEACCOUNT_NAME = :DEVICEACCOUNT_NAME" + i + " , ");
                            strSql.Append(" DEVICEACCOUNT_ID = :DEVICEACCOUNT_ID" + i + " , ");
                            strSql.Append(" TEAM = :TEAM" + i + " , ");
                            strSql.Append(" WORKING_STATE = :WORKING_STATE" + i + " , ");
                            strSql.Append(" FAULT_DESCRIPTION = :FAULT_DESCRIPTION" + i);
                            //strSql.Append(" DOWN_WELL_SEQUENCE = :DOWN_WELL_SEQUENCE  ");
                            strSql.Append(" where EQUIPID=:EQUIPID" + i + ";");
                            parameters.Add(ServiceUtils.CreateOracleParameter(":EQUIPID" + i, OracleDbType.Varchar2, model3[i].EQUIPID));
                        }
                        parameters.AddRange(new OracleParameter[] {
			            //ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, model.JOB_PLAN_CD) ,            
                        ServiceUtils.CreateOracleParameter(":DEVICEACCOUNT_NAME" + i, OracleDbType.Varchar2, model3[i].DEVICEACCOUNT_NAME) ,            
                        ServiceUtils.CreateOracleParameter(":DEVICEACCOUNT_ID" + i, OracleDbType.Varchar2, model3[i].DEVICEACCOUNT_ID) ,            
                        ServiceUtils.CreateOracleParameter(":TEAM" + i, OracleDbType.Varchar2, model3[i].TEAM) ,            
                        ServiceUtils.CreateOracleParameter(":WORKING_STATE" + i, OracleDbType.Varchar2, model3[i].WORKING_STATE) ,            
                        ServiceUtils.CreateOracleParameter(":FAULT_DESCRIPTION" + i, OracleDbType.Varchar2, model3[i].FAULT_DESCRIPTION) });

                    }
                }
            }
            if (dataRemoveList_井下设备 != null)
            {
                var model4 = Utility.ModelHelper.DeserializeObject(dataRemoveList_井下设备) as List<Model.DM_LOG_DOWNHOLE_EQUIP>;
                if (model4 != null)
                {
                    for (int i = 0; i < model4.Count; ++i)
                    {
                        strSql.Append("delete from DM_LOG_DOWNHOLE_EQUIP ");
                        strSql.Append(" where EQUIPID=:DEL_EQUIPID" + i + ";");
                        parameters.Add(ServiceUtils.CreateOracleParameter(":DEL_EQUIPID" + i, OracleDbType.Varchar2, model4[i].EQUIPID));
                    }
                }
            }
            strSql.Append(" end;");
            if (DbHelperOra.ExecuteSqlTran(strSql.ToString(), parameters.ToArray()) > 0) return true;
            return false;
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetData_项目明细(string JOB_PLAN_CD, string DOWN_WELL_SEQUENCE)
        {
            DataSet ds = new DataSet();
            DataTable dt1 = DbHelperOra.Query("select * from dm_log_work_details where job_plan_cd=:JOB_PLAN_CD and down_well_sequence=:DOWN_WELL_SEQUENCE",
                new OracleParameter[]{
                    ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, JOB_PLAN_CD),
                    ServiceUtils.CreateOracleParameter(":DOWN_WELL_SEQUENCE", OracleDbType.Decimal, DOWN_WELL_SEQUENCE)}).Tables[0];
            DataTable dt2 = DbHelperOra.Query("select * from dm_log_work_holdup_details where job_plan_cd=:JOB_PLAN_CD and down_well_sequence=:DOWN_WELL_SEQUENCE",
                new OracleParameter[]{
                    ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, JOB_PLAN_CD),
                    ServiceUtils.CreateOracleParameter(":DOWN_WELL_SEQUENCE", OracleDbType.Decimal, DOWN_WELL_SEQUENCE)}).Tables[0];

            DataTable dt3 = DbHelperOra.Query("select * from DM_LOG_RADIATION_STATUS where job_plan_cd=:JOB_PLAN_CD and down_well_sequence=:DOWN_WELL_SEQUENCE",
    new OracleParameter[]{
                    ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, JOB_PLAN_CD),
                    ServiceUtils.CreateOracleParameter(":DOWN_WELL_SEQUENCE", OracleDbType.Decimal, DOWN_WELL_SEQUENCE)}).Tables[0];

            DataTable dt4 = DbHelperOra.Query("select * from DM_LOG_ITEMS where job_plan_cd=:JOB_PLAN_CD and down_well_sequence=:DOWN_WELL_SEQUENCE",
    new OracleParameter[]{
                    ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, JOB_PLAN_CD),
                    ServiceUtils.CreateOracleParameter(":DOWN_WELL_SEQUENCE", OracleDbType.Decimal, DOWN_WELL_SEQUENCE)}).Tables[0];

            DataTable dt5 = DbHelperOra.Query("select * from DM_LOG_DOWNHOLE_EQUIP where job_plan_cd=:JOB_PLAN_CD and down_well_sequence=:DOWN_WELL_SEQUENCE",
    new OracleParameter[]{
                    ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, JOB_PLAN_CD),
                    ServiceUtils.CreateOracleParameter(":DOWN_WELL_SEQUENCE", OracleDbType.Decimal, DOWN_WELL_SEQUENCE)}).Tables[0];

            dt1.TableName = "DM_LOG_WORK_DETAILS";
            dt2.TableName = "DM_LOG_WORK_HOLDUP_DETAILS";
            dt3.TableName = "DM_LOG_RADIATION_STATUS";
            dt4.TableName = "DM_LOG_ITEMS";
            dt5.TableName = "DM_LOG_DOWNHOLE_EQUIP";

            ds.Tables.Add(dt1.Copy());
            ds.Tables.Add(dt2.Copy());
            ds.Tables.Add(dt3.Copy());
            ds.Tables.Add(dt4.Copy());
            ds.Tables.Add(dt5.Copy());
            return ds;
        }
        [WebMethod(EnableSession = true)]
        public DataSet GetData_专家指导(string JOB_PLAN_CD, string DOWN_WELL_SEQUENCE)
        {
            DataSet ds = new DataSet();
            DataTable dt1 = DbHelperOra.Query("select * from PRO_LOG_REMOTE_DIRECT where job_plan_cd=:JOB_PLAN_CD and down_well_sequence=:DOWN_WELL_SEQUENCE",
                new OracleParameter[]{
                    ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, JOB_PLAN_CD),
                    ServiceUtils.CreateOracleParameter(":DOWN_WELL_SEQUENCE", OracleDbType.Decimal, DOWN_WELL_SEQUENCE)}).Tables[0];
            DataTable dt2 = DbHelperOra.Query("select * from PRO_LOG_PUNISH where job_plan_cd=:JOB_PLAN_CD and down_well_sequence=:DOWN_WELL_SEQUENCE",
                new OracleParameter[]{
                    ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, JOB_PLAN_CD),
                    ServiceUtils.CreateOracleParameter(":DOWN_WELL_SEQUENCE", OracleDbType.Decimal, DOWN_WELL_SEQUENCE)}).Tables[0];

            dt1.TableName = "PRO_LOG_REMOTE_DIRECT";
            dt2.TableName = "PRO_LOG_PUNISH";

            ds.Tables.Add(dt1.Copy());
            ds.Tables.Add(dt2.Copy());
            return ds;
        }
        [WebMethod(EnableSession = true)]
        public bool Save_专家指导(byte[] data_远程指导, byte[] data_测井监督)
        {
            if (data_测井监督 == null || data_远程指导 == null) return false;
            var model_远程指导 = Utility.ModelHelper.DeserializeObject(data_远程指导) as Model.PRO_LOG_REMOTE_DIRECT;
            var model_测井监督 = Utility.ModelHelper.DeserializeObject(data_测井监督) as Model.PRO_LOG_PUNISH;

            Workflow.Controller.ValidateSave<Workflow.C测井现场提交信息>(model_远程指导.JOB_PLAN_CD);

            StringBuilder strSql = new StringBuilder();
            List<OracleParameter> parameters = new List<OracleParameter>();

            strSql.Append("begin ");
            if (model_测井监督.DOWN_WELL_SEQUENCE == null)
            {
                strSql.Append("insert into PRO_LOG_REMOTE_DIRECT(");
                strSql.Append("DIRECTID,JOB_PLAN_CD,DOWN_WELL_SEQUENCE,EXPERTISE,EXPERT_LEADER,DIRECT_DATE,DIRECT_MEANS,EXPERTISE_RECEIVE_MAN,EXPERTISE_EFFECT,NOTE");
                strSql.Append(") values (");
                strSql.Append("DIRECTID_SEQUENCE.nextval,:JOB_PLAN_CD,:DOWN_WELL_SEQUENCE,:EXPERTISE,:EXPERT_LEADER,:DIRECT_DATE,:DIRECT_MEANS,:EXPERTISE_RECEIVE_MAN,:EXPERTISE_EFFECT,:NOTE");
                strSql.Append("); ");

                strSql.Append("insert into PRO_LOG_PUNISH(");
                strSql.Append("PUNISHID,JOB_PLAN_CD,REASON,PUNISH_DATE,PUNISH_SUGGESTION,IS_RECTIFY,REVISE_DATE,LOG_SUPERVISION_ID,NOTE,DOWN_WELL_SEQUENCE");
                strSql.Append(") values (");
                strSql.Append("PUNISHID_SEQUENCE.nextval,:JOB_PLAN_CD,:REASON,:PUNISH_DATE,:PUNISH_SUGGESTION,:IS_RECTIFY,:REVISE_DATE,:LOG_SUPERVISION_ID,:NOTE1,:DOWN_WELL_SEQUENCE");
                strSql.Append(") ;");

                parameters.Add(ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, model_远程指导.JOB_PLAN_CD));
                parameters.Add(ServiceUtils.CreateOracleParameter(":DOWN_WELL_SEQUENCE", OracleDbType.Decimal, model_远程指导.DOWN_WELL_SEQUENCE));
            }
            else
            {
                strSql.Append("update PRO_LOG_REMOTE_DIRECT set ");
                //strSql.Append(" DIRECTID = :DIRECTID , ");
                //strSql.Append(" JOB_PLAN_CD = :JOB_PLAN_CD , ");
                //strSql.Append(" DOWN_WELL_SEQUENCE = :DOWN_WELL_SEQUENCE , ");
                strSql.Append(" EXPERTISE = :EXPERTISE , ");
                strSql.Append(" EXPERT_LEADER = :EXPERT_LEADER , ");
                strSql.Append(" DIRECT_DATE = :DIRECT_DATE , ");
                strSql.Append(" DIRECT_MEANS = :DIRECT_MEANS , ");
                strSql.Append(" EXPERTISE_RECEIVE_MAN = :EXPERTISE_RECEIVE_MAN , ");
                strSql.Append(" EXPERTISE_EFFECT = :EXPERTISE_EFFECT , ");
                strSql.Append(" NOTE = :NOTE  ");
                strSql.Append(" where DIRECTID=:DIRECTID;");

                strSql.Append("update PRO_LOG_PUNISH set ");
                //strSql.Append(" PUNISHID = :PUNISHID , ");
                //strSql.Append(" JOB_PLAN_CD = :JOB_PLAN_CD , ");
                strSql.Append(" REASON = :REASON , ");
                strSql.Append(" PUNISH_DATE = :PUNISH_DATE , ");
                strSql.Append(" PUNISH_SUGGESTION = :PUNISH_SUGGESTION , ");
                strSql.Append(" IS_RECTIFY = :IS_RECTIFY , ");
                strSql.Append(" REVISE_DATE = :REVISE_DATE , ");
                strSql.Append(" LOG_SUPERVISION_ID = :LOG_SUPERVISION_ID , ");
                strSql.Append(" NOTE = :NOTE1 ");
                // strSql.Append(" DOWN_WELL_SEQUENCE = :DOWN_WELL_SEQUENCE  ");
                strSql.Append(" where PUNISHID=:PUNISHID ;");

                parameters.Add(ServiceUtils.CreateOracleParameter(":DIRECTID", OracleDbType.Decimal, model_远程指导.DIRECTID));
                parameters.Add(ServiceUtils.CreateOracleParameter(":PUNISHID", OracleDbType.Decimal, model_测井监督.PUNISHID));
            }
            parameters.AddRange(new OracleParameter[] {
                        ServiceUtils.CreateOracleParameter(":EXPERTISE", OracleDbType.NVarchar2, model_远程指导.EXPERTISE) ,            
                        ServiceUtils.CreateOracleParameter(":EXPERT_LEADER", OracleDbType.NVarchar2, model_远程指导.EXPERT_LEADER) ,  
                        ServiceUtils.CreateOracleParameter(":DIRECT_DATE", OracleDbType.Date, model_远程指导.DIRECT_DATE) ,   
                        ServiceUtils.CreateOracleParameter(":DIRECT_MEANS", OracleDbType.Varchar2, model_远程指导.DIRECT_MEANS) , 
                        ServiceUtils.CreateOracleParameter(":EXPERTISE_RECEIVE_MAN", OracleDbType.NVarchar2, model_远程指导.EXPERTISE_RECEIVE_MAN) ,
                        ServiceUtils.CreateOracleParameter(":EXPERTISE_EFFECT", OracleDbType.NVarchar2, model_远程指导.EXPERTISE_EFFECT) ,
                        ServiceUtils.CreateOracleParameter(":NOTE", OracleDbType.NVarchar2, model_远程指导.NOTE)});

            parameters.AddRange(new OracleParameter[] {
                        ServiceUtils.CreateOracleParameter(":REASON", OracleDbType.NVarchar2, model_测井监督.REASON) ,            
                        ServiceUtils.CreateOracleParameter(":PUNISH_DATE", OracleDbType.Date, model_测井监督.PUNISH_DATE) ,      
                        ServiceUtils.CreateOracleParameter(":PUNISH_SUGGESTION", OracleDbType.NVarchar2, model_测井监督.PUNISH_SUGGESTION) , 
                        ServiceUtils.CreateOracleParameter(":IS_RECTIFY", OracleDbType.NVarchar2, model_测井监督.IS_RECTIFY) ,            
                        ServiceUtils.CreateOracleParameter(":REVISE_DATE", OracleDbType.Date, model_测井监督.REVISE_DATE) ,         
                        ServiceUtils.CreateOracleParameter(":LOG_SUPERVISION_ID", OracleDbType.Char, model_测井监督.LOG_SUPERVISION_ID) ,
                        ServiceUtils.CreateOracleParameter(":NOTE1", OracleDbType.NVarchar2, model_测井监督.NOTE)});
            strSql.Append(" end;");
            if (DbHelperOra.ExecuteSqlTran(strSql.ToString(), parameters.ToArray()) > 0) return true;
            return false;
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetProcessWellJob()
        {
            return DbHelperOra.Query("select distinct a.drill_job_id,a.well_job_name from com_job_info a,dm_log_task b,dm_log_ops_plan c,sys_work_flow_now d where a.drill_job_id=b.drill_job_id and b.requisition_cd=c.requisition_cd and c.job_plan_cd=d.obj_id and d.flow_type=" + (int)ServiceEnums.WorkflowType.测井现场提交信息 + " and d.flow_state=" + (int)ServiceEnums.WorkflowState.数据指派 + " and d.target_loginname=:TARGET_LOGINNAME order by a.well_job_name",
                new OracleParameter[] {        
                   ServiceUtils.CreateOracleParameter(":TARGET_LOGINNAME", OracleDbType.Varchar2, ServiceUtils.GetUserInfo().COL_LOGINNAME)
            });
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetProcessDataJobSource(string job_id)
        {
            return DbHelperOra.Query("select distinct c.job_plan_cd from dm_log_task b,dm_log_ops_plan c,sys_work_flow_now d where b.drill_job_id=:DRILL_JOB_ID and b.requisition_cd=c.requisition_cd and c.job_plan_cd=d.obj_id and d.target_loginname=:TARGET_LOGINNAME and d.flow_type=" + (int)ServiceEnums.WorkflowType.测井现场提交信息 + " and d.flow_state=" + (int)ServiceEnums.WorkflowState.数据指派 + " order by c.job_plan_cd",
                new OracleParameter[] {
                    ServiceUtils.CreateOracleParameter(":DRILL_JOB_ID", OracleDbType.Char, job_id),
                    ServiceUtils.CreateOracleParameter(":TARGET_LOGINNAME", OracleDbType.Varchar2, ServiceUtils.GetUserInfo().COL_LOGINNAME)
                });
        }

        [WebMethod(EnableSession = true)]
        public bool Save_解释处理作业(byte[] byte_DM_LOG_PROCESS, byte[] byte_DM_LOG_SOURCE_DATA, byte[] byte_PRO_LOG_DATA_PUBLISH, DataTable processItems, DataTable processMaps, DataTable processCurveRating, DataTable originalMaps)
        {
            var model1 = Utility.ModelHelper.DeserializeObject(byte_DM_LOG_PROCESS) as Model.DM_LOG_PROCESS;
            if (string.IsNullOrWhiteSpace(model1.PROCESS_NAME)) return false;
            var model2 = Utility.ModelHelper.DeserializeObject(byte_DM_LOG_SOURCE_DATA) as List<Model.DM_LOG_SOURCE_DATA>;
            var model3 = Utility.ModelHelper.DeserializeObject(byte_PRO_LOG_DATA_PUBLISH) as Model.PRO_LOG_DATA_PUBLISH;

            var process_id = DbHelperOra.GetSingle("select PROCESS_ID from dm_log_process where process_name=:PROCESS_NAME",
                    ServiceUtils.CreateOracleParameter(":PROCESS_NAME", OracleDbType.Varchar2, model1.PROCESS_NAME));
            if (process_id != null && string.IsNullOrEmpty(model1.PROCESS_ID))
                ServiceUtils.ThrowSoapException("解释处理作业已经存在！");

            Workflow.Controller.ValidateSave<Workflow.C解释处理作业>(model1.PROCESS_ID);

            StringBuilder strSql = new StringBuilder();
            var parameters = new List<OracleParameter>();
            strSql.Append("begin ");
            if (process_id == null)
            {
                string well_job_name = DbHelperOra.GetSingle("select well_job_name from com_job_info where drill_job_id=" + model1.DRILL_JOB_ID) as string;
                if (!model1.PROCESS_NAME.StartsWith(well_job_name + "#"))
                    ServiceUtils.ThrowSoapException("解释处理作业名与作业井名不匹配！");

                process_id = DbHelperOra.GetSingle("select PROCESS_ID_SEQ.nextval from dual");
                strSql.Append("insert into DM_LOG_PROCESS(");
                strSql.Append("PROCESS_ID,LOG_DATA_ID,WELL_ID,DRILL_JOB_ID,PROCESS_NAME,PROCESS_CODE,PROCESS_START_DATE,NOTE");
                strSql.Append(") values (");
                strSql.Append(":PROCESS_ID,:LOG_DATA_ID,:WELL_ID,:DRILL_JOB_ID,:PROCESS_NAME,:PROCESS_CODE,:PROCESS_START_DATE,:NOTE");
                strSql.Append(");");

                strSql.Append("insert into PRO_LOG_DATA_PUBLISH(");
                strSql.Append("DATA_PUBLISH_ID,PROCESS_ID,INTERPRET_CENTER,LOG_START_TIME,LOG_TOTAL_TIME,LOST_TIME,TEAM_ORG_ID,LOG_SERIES_ID,PRO_START_TIME,P_TOTAL_TIME,P_PROCESS_SOFTWARE,PROCESSOR,INTERPRETER,P_SUPERVISOR,RESULT_MAP_TYPE,LOG_ORIGINALITY_DATA,LOG_INTERPRET_REPORT,LOG_INTERPRET_RESULT,NOTE,FILE_NUMBER,P_SCENE_RATING,P_INDOOR_RATING,ACCEPTANCE_WAY_NAME");
                strSql.Append(") values (");
                strSql.Append("PUBLISH_ID_SEQ.nextval,:PROCESS_ID,:INTERPRET_CENTER,:LOG_START_TIME,:LOG_TOTAL_TIME,:LOST_TIME,:TEAM_ORG_ID,:LOG_SERIES_ID,:PRO_START_TIME,:P_TOTAL_TIME,:P_PROCESS_SOFTWARE,:PROCESSOR,:INTERPRETER,:P_SUPERVISOR,:RESULT_MAP_TYPE,:LOG_ORIGINALITY_DATA,:LOG_INTERPRET_REPORT,:LOG_INTERPRET_RESULT,:NOTE1,:FILE_NUMBER,:P_SCENE_RATING,:P_INDOOR_RATING,:ACCEPTANCE_WAY_NAME");
                strSql.Append(");");

                strSql.Append("insert into SYS_WORK_FLOW(");
                strSql.Append("FLOW_ID,OBJ_ID,SOURCE_LOGINNAME,FLOW_TYPE,FLOW_STATE");
                strSql.Append(") values (");
                strSql.Append("FLOW_ID_SEQ.NEXTVAL,:PROCESS_ID,:SOURCE_LOGINNAME," + (int)ServiceEnums.WorkflowType.解释处理作业 + "," + (int)ServiceEnums.WorkflowState.新建);
                strSql.Append(");");
                parameters.Add(ServiceUtils.CreateOracleParameter(":SOURCE_LOGINNAME", OracleDbType.Varchar2, ServiceUtils.GetUserInfo().COL_LOGINNAME));
                parameters.Add(ServiceUtils.CreateOracleParameter(":PROCESS_NAME", OracleDbType.Varchar2, model1.PROCESS_NAME));
            }
            else
            {
                strSql.Append("update DM_LOG_PROCESS set ");
                //strSql.Append(" PROCESS_ID = :PROCESS_ID , ");
                strSql.Append(" LOG_DATA_ID = :LOG_DATA_ID , ");
                strSql.Append(" WELL_ID = :WELL_ID , ");
                strSql.Append(" DRILL_JOB_ID = :DRILL_JOB_ID , ");
                //strSql.Append(" PROCESS_NAME = :PROCESS_NAME , ");
                strSql.Append(" PROCESS_CODE = :PROCESS_CODE , ");
                strSql.Append(" PROCESS_START_DATE = :PROCESS_START_DATE , ");
                strSql.Append(" NOTE = :NOTE  ");
                strSql.Append(" where PROCESS_ID=:PROCESS_ID;");

                strSql.Append("update PRO_LOG_DATA_PUBLISH set ");
                //strSql.Append(" DATA_PUBLISH_ID = :DATA_PUBLISH_ID , ");
                //strSql.Append(" PROCESS_ID = :PROCESS_ID , ");
                strSql.Append(" INTERPRET_CENTER = :INTERPRET_CENTER , ");
                strSql.Append(" LOG_START_TIME = :LOG_START_TIME , ");
                strSql.Append(" LOG_TOTAL_TIME = :LOG_TOTAL_TIME , ");
                strSql.Append(" LOST_TIME = :LOST_TIME , ");
                strSql.Append(" TEAM_ORG_ID = :TEAM_ORG_ID , ");
                strSql.Append(" LOG_SERIES_ID = :LOG_SERIES_ID , ");
                strSql.Append(" PRO_START_TIME = :PRO_START_TIME , ");
                strSql.Append(" P_TOTAL_TIME = :P_TOTAL_TIME , ");
                strSql.Append(" P_PROCESS_SOFTWARE = :P_PROCESS_SOFTWARE , ");
                strSql.Append(" PROCESSOR = :PROCESSOR , ");
                strSql.Append(" INTERPRETER = :INTERPRETER , ");
                strSql.Append(" P_SUPERVISOR = :P_SUPERVISOR , ");
                strSql.Append(" RESULT_MAP_TYPE = :RESULT_MAP_TYPE , ");
                strSql.Append(" LOG_ORIGINALITY_DATA = :LOG_ORIGINALITY_DATA , ");
                strSql.Append(" LOG_INTERPRET_REPORT = :LOG_INTERPRET_REPORT , ");
                strSql.Append(" LOG_INTERPRET_RESULT = :LOG_INTERPRET_RESULT , ");
                strSql.Append(" NOTE = :NOTE1 , ");
                strSql.Append(" FILE_NUMBER = :FILE_NUMBER,  ");
                strSql.Append(" P_SCENE_RATING = :P_SCENE_RATING, ");
                strSql.Append(" P_INDOOR_RATING = :P_INDOOR_RATING, ");
                strSql.Append(" ACCEPTANCE_WAY_NAME=:ACCEPTANCE_WAY_NAME ");
                strSql.Append(" where PROCESS_ID=:PROCESS_ID;");
            }
            parameters.AddRange(new OracleParameter[] {
                    ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id),
                    ServiceUtils.CreateOracleParameter(":LOG_DATA_ID", OracleDbType.Varchar2, model1.LOG_DATA_ID) ,            
                    ServiceUtils.CreateOracleParameter(":WELL_ID", OracleDbType.Char, model1.WELL_ID) ,            
                    ServiceUtils.CreateOracleParameter(":DRILL_JOB_ID", OracleDbType.Varchar2, model1.DRILL_JOB_ID) ,            
                    //ServiceUtils.CreateOracleParameter(":PROCESS_NAME", OracleDbType.Varchar2, model1.PROCESS_NAME) ,            
                    ServiceUtils.CreateOracleParameter(":PROCESS_CODE", OracleDbType.Char, model1.PROCESS_CODE) ,            
                    ServiceUtils.CreateOracleParameter(":PROCESS_START_DATE", OracleDbType.Date, model1.PROCESS_START_DATE) ,            
                    ServiceUtils.CreateOracleParameter(":NOTE", OracleDbType.Varchar2, model1.NOTE) ,

			        //ServiceUtils.CreateOracleParameter(":DATA_PUBLISH_ID", OracleDbType.Decimal, model3.DATA_PUBLISH_ID) ,            
                    //ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, model3.PROCESS_ID) ,            
                    ServiceUtils.CreateOracleParameter(":INTERPRET_CENTER", OracleDbType.Char, model3.INTERPRET_CENTER) ,            
                    ServiceUtils.CreateOracleParameter(":LOG_START_TIME", OracleDbType.Date, model3.LOG_START_TIME) ,            
                    ServiceUtils.CreateOracleParameter(":LOG_TOTAL_TIME", OracleDbType.Decimal, model3.LOG_TOTAL_TIME) ,            
                    ServiceUtils.CreateOracleParameter(":LOST_TIME", OracleDbType.Decimal, model3.LOST_TIME) ,            
                    ServiceUtils.CreateOracleParameter(":TEAM_ORG_ID", OracleDbType.Varchar2, model3.TEAM_ORG_ID) ,            
                    ServiceUtils.CreateOracleParameter(":LOG_SERIES_ID", OracleDbType.Varchar2, model3.LOG_SERIES_ID) ,            
                    ServiceUtils.CreateOracleParameter(":PRO_START_TIME", OracleDbType.Date, model3.PRO_START_TIME) ,            
                    ServiceUtils.CreateOracleParameter(":P_TOTAL_TIME", OracleDbType.Decimal, model3.P_TOTAL_TIME) ,            
                    ServiceUtils.CreateOracleParameter(":P_PROCESS_SOFTWARE", OracleDbType.Varchar2, model3.P_PROCESS_SOFTWARE) ,            
                    ServiceUtils.CreateOracleParameter(":PROCESSOR", OracleDbType.Varchar2, model3.PROCESSOR) ,            
                    ServiceUtils.CreateOracleParameter(":INTERPRETER", OracleDbType.Varchar2, model3.INTERPRETER) ,            
                    ServiceUtils.CreateOracleParameter(":P_SUPERVISOR", OracleDbType.Varchar2, model3.P_SUPERVISOR) ,            
                    ServiceUtils.CreateOracleParameter(":RESULT_MAP_TYPE", OracleDbType.Decimal, model3.RESULT_MAP_TYPE) ,            
                    ServiceUtils.CreateOracleParameter(":LOG_ORIGINALITY_DATA", OracleDbType.Decimal, model3.LOG_ORIGINALITY_DATA) ,            
                    ServiceUtils.CreateOracleParameter(":LOG_INTERPRET_REPORT", OracleDbType.Decimal, model3.LOG_INTERPRET_REPORT) ,            
                    ServiceUtils.CreateOracleParameter(":LOG_INTERPRET_RESULT", OracleDbType.Decimal, model3.LOG_INTERPRET_RESULT) ,            
                    ServiceUtils.CreateOracleParameter(":NOTE1", OracleDbType.Varchar2, model3.NOTE),
                    ServiceUtils.CreateOracleParameter(":FILE_NUMBER", OracleDbType.Decimal, model3.FILE_NUMBER)  ,
                    ServiceUtils.CreateOracleParameter(":P_SCENE_RATING", OracleDbType.Varchar2, model3.P_SCENE_RATING)  ,
                    ServiceUtils.CreateOracleParameter(":P_INDOOR_RATING", OracleDbType.Varchar2, model3.P_INDOOR_RATING),
                    ServiceUtils.CreateOracleParameter(":ACCEPTANCE_WAY_NAME",OracleDbType.Varchar2,model3.ACCEPTANCE_WAY_NAME)
                     
                });
            if (model2 != null && model2.Count > 0)
            {
                strSql.Append("delete from DM_LOG_SOURCE_DATA ");
                strSql.Append(" where LOG_DATA_ID=:PROCESS_ID;");
                for (int i = 0; i < model2.Count; i++)
                {
                    strSql.Append("insert into DM_LOG_SOURCE_DATA(");
                    strSql.Append("LOG_DATA_ID,JOB_PLAN_CD,NOTE");
                    strSql.Append(") values (");
                    strSql.Append(":PROCESS_ID,:JOB_PLAN_CD" + i + ",:NOTE2" + i);
                    strSql.Append(");");

                    parameters.AddRange(new OracleParameter[] {
			            //ServiceUtils.CreateOracleParameter(":LOG_DATA_ID", OracleDbType.Varchar2, model2.LOG_DATA_ID) ,            
                        ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD"+i, OracleDbType.Varchar2, model2[i].JOB_PLAN_CD) ,            
                        ServiceUtils.CreateOracleParameter(":NOTE2"+i, OracleDbType.Varchar2, model2[i].NOTE)             
                    });
                }
            }
            strSql.Append("END;");
            if (DbHelperOra.ExecuteSqlTran(strSql.ToString(), parameters.ToArray()) < 1) return false;
            strSql.Clear();

            object maxid = DbHelperOra.GetSingle("select max(ITEMID) from PRO_LOG_PROCESSING_ITEM where PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter("PROCESS_ID", OracleDbType.Varchar2, process_id));
            strSql.Append("insert into PRO_LOG_PROCESSING_ITEM(");
            strSql.Append("ITEMID,PROCESS_ID,PROCESSING_ITEM_ID,P_CURVE_NUMBER,P_PROCESS_SOFTWARE,P_WELL_INTERVAL,DATA_NAME,PROCESSOR,P_START_DATE,SCALE,INTERPRETER,P_SUPERVISOR,NOTE,LOG_SERIES_NAME,LOG_DATA_FORMAT");
            strSql.Append(") values (");
            strSql.Append("ITEMID_SEQ.nextval,:PROCESS_ID,:PROCESSING_ITEM_ID,:P_CURVE_NUMBER,:P_PROCESS_SOFTWARE,:P_WELL_INTERVAL,:DATA_NAME,:PROCESSOR,:P_START_DATE,:SCALE,:INTERPRETER,:P_SUPERVISOR,:NOTE,:LOG_SERIES_NAME,:LOG_DATA_FORMAT");
            strSql.Append(") ");
            DbHelperOra.InsertDataTable(strSql.ToString(), processItems,
                        ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id),
                        ServiceUtils.CreateOracleParameter(":PROCESSING_ITEM_ID", "PROCESSING_ITEM_ID", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":P_CURVE_NUMBER", "P_CURVE_NUMBER", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":P_PROCESS_SOFTWARE", "P_PROCESS_SOFTWARE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":P_WELL_INTERVAL", "P_WELL_INTERVAL", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":DATA_NAME", "DATA_NAME", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":PROCESSOR", "PROCESSOR", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":P_START_DATE", "P_START_DATE", OracleDbType.TimeStamp),
                        ServiceUtils.CreateOracleParameter(":SCALE", "SCALE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":INTERPRETER", "INTERPRETER", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":P_SUPERVISOR", "P_SUPERVISOR", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":NOTE", "NOTE", OracleDbType.NVarchar2),
                        ServiceUtils.CreateOracleParameter(":LOG_SERIES_NAME", "LOG_SERIES_NAME", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":LOG_DATA_FORMAT", "LOG_DATA_FORMAT", OracleDbType.Varchar2));

            DbHelperOra.ExecuteSql("delete from PRO_LOG_PROCESSING_ITEM where PROCESS_ID=:PROCESS_ID and ITEMID<=:ITEMID",
    ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id),
    ServiceUtils.CreateOracleParameter(":ITEMID", OracleDbType.Decimal, maxid)
    );
            maxid = DbHelperOra.GetSingle("select max(MAPID) from PRO_LOG_PROCESS_MAP where PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter("PROCESS_ID", OracleDbType.Varchar2, process_id));
            strSql.Clear();
            strSql.Append("insert into PRO_LOG_PROCESS_MAP(");
            strSql.Append("MAPID,PROCESS_ID,PROCESSING_ITEM_ID,MAPS_CODING,MAP_START_DEP,MAP_END_DEP,MAP_SCALE,MAP_DATA_NAME,MAP_PDF_SIZE,MAP_PDF_DATA,P_PROCESS_SOFTWARE,MAP_OUT_DATE,MAP_PERSON,MAP_NUMBER,MAP_TEMPLATE_SIZE,MAP_TEMPLATE_DATA,MAP_VERIFIER");
            strSql.Append(") values (");
            strSql.Append("MAPID_SEQ.nextval,:PROCESS_ID,:PROCESSING_ITEM_ID,:MAPS_CODING,:MAP_START_DEP,:MAP_END_DEP,:MAP_SCALE,:MAP_DATA_NAME,:MAP_PDF_SIZE,:MAP_PDF_DATA,:P_PROCESS_SOFTWARE,:MAP_OUT_DATE,:MAP_PERSON,:MAP_NUMBER,:MAP_TEMPLATE_SIZE,:MAP_TEMPLATE_DATA,:MAP_VERIFIER");
            strSql.Append(") ");
            DbHelperOra.InsertDataTable(strSql.ToString(), processMaps,
                ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id),
                ServiceUtils.CreateOracleParameter(":PROCESSING_ITEM_ID", "PROCESSING_ITEM_ID", OracleDbType.Decimal),
                ServiceUtils.CreateOracleParameter(":MAPS_CODING", "MAPS_CODING", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":MAP_START_DEP", "MAP_START_DEP", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":MAP_END_DEP", "MAP_END_DEP", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":MAP_SCALE", "MAP_SCALE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":MAP_DATA_NAME", "MAP_DATA_NAME", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":MAP_PDF_SIZE", "MAP_PDF_SIZE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":MAP_PDF_DATA", "MAP_PDF_DATA", OracleDbType.Blob),
                        ServiceUtils.CreateOracleParameter(":P_PROCESS_SOFTWARE", "P_PROCESS_SOFTWARE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":MAP_OUT_DATE", "MAP_OUT_DATE", OracleDbType.TimeStamp),
                        ServiceUtils.CreateOracleParameter(":MAP_PERSON", "MAP_PERSON", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":MAP_NUMBER", "MAP_NUMBER", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":MAP_TEMPLATE_SIZE", "MAP_TEMPLATE_SIZE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":MAP_TEMPLATE_DATA", "MAP_TEMPLATE_DATA", OracleDbType.Blob),
                        ServiceUtils.CreateOracleParameter(":MAP_VERIFIER", "MAP_VERIFIER", OracleDbType.Varchar2));


            DbHelperOra.ExecuteSql("delete from PRO_LOG_PROCESS_MAP where PROCESS_ID=:PROCESS_ID and MAPID<=:MAPID",
               ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id),
               ServiceUtils.CreateOracleParameter(":MAPID", OracleDbType.Decimal, maxid));

            maxid = DbHelperOra.GetSingle("select max(ROWID) from PRO_LOG_PROCESSING_CURVERATING where PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter("PROCESS_ID", OracleDbType.Varchar2, process_id));
            DbHelperOra.ExecuteSql("delete from PRO_LOG_PROCESSING_CURVERATING where PROCESS_ID=:PROCESS_ID and ROWID<=:ROW_ID",
               ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id),
               ServiceUtils.CreateOracleParameter(":ROW_ID", OracleDbType.Varchar2, maxid));
            strSql.Clear();
            strSql.Append("insert into PRO_LOG_PROCESSING_CURVERATING(");
            strSql.Append("PROCESS_ID,PROCESSING_ITEM_ID,SCALE,RLEV,SCENE_RATING,INDOOR_RATING,CURVE_ID,START_DEP,END_DEP,WHY");
            strSql.Append(") values (");
            strSql.Append(":PROCESS_ID,:PROCESSING_ITEM_ID,:SCALE,:RLEV,:SCENE_RATING,:INDOOR_RATING,:CURVE_ID,:START_DEP,:END_DEP,:WHY");
            strSql.Append(") ");
            DbHelperOra.InsertDataTable(strSql.ToString(), processCurveRating,
                ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id),
                ServiceUtils.CreateOracleParameter(":PROCESSING_ITEM_ID", "PROCESSING_ITEM_ID", OracleDbType.Decimal),
                ServiceUtils.CreateOracleParameter(":SCALE", "SCALE", OracleDbType.Varchar2),
                ServiceUtils.CreateOracleParameter(":RLEV", "RLEV", OracleDbType.Decimal),
                ServiceUtils.CreateOracleParameter(":SCENE_RATING", "SCENE_RATING", OracleDbType.Varchar2),
                ServiceUtils.CreateOracleParameter(":INDOOR_RATING", "INDOOR_RATING", OracleDbType.Varchar2),
                ServiceUtils.CreateOracleParameter(":CURVE_ID", "CURVE_ID", OracleDbType.Decimal),
                ServiceUtils.CreateOracleParameter(":START_DEP", "START_DEP", OracleDbType.Decimal),
                ServiceUtils.CreateOracleParameter(":END_DEP", "END_DEP", OracleDbType.Decimal),
                ServiceUtils.CreateOracleParameter(":WHY", "WHY", OracleDbType.Varchar2));

            maxid = DbHelperOra.GetSingle("select max(MAPID) from PRO_LOG_ORIGINAL_MAP where PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter("PROCESS_ID", OracleDbType.Varchar2, process_id));
            strSql.Clear();
            strSql.Append("insert into PRO_LOG_ORIGINAL_MAP(");
            strSql.Append("MAPID,PROCESS_ID,MAPS_NAME,MAP_START_DEP,MAP_END_DEP,MAP_SCALE,MAP_DATA_NAME,REWORK_NUM,REWORK_WHY,MAP_DATE,REVIEWER,NOTE");
            strSql.Append(") values (");
            strSql.Append("ORIGINAL_MAPID_SEQ.nextval,:PROCESS_ID,:MAPS_NAME,:MAP_START_DEP,:MAP_END_DEP,:MAP_SCALE,:MAP_DATA_NAME,:REWORK_NUM,:REWORK_WHY,:MAP_DATE,:REVIEWER,:NOTE");
            strSql.Append(") ");
            DbHelperOra.InsertDataTable(strSql.ToString(), originalMaps,
                ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id),
                ServiceUtils.CreateOracleParameter(":MAPS_NAME", "MAPS_NAME", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":MAP_START_DEP", "MAP_START_DEP", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":MAP_END_DEP", "MAP_END_DEP", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":MAP_SCALE", "MAP_SCALE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":MAP_DATA_NAME", "MAP_DATA_NAME", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":REWORK_NUM", "REWORK_NUM", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":REWORK_WHY", "REWORK_WHY", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":MAP_DATE", "MAP_DATE", OracleDbType.TimeStamp),
                        ServiceUtils.CreateOracleParameter(":REVIEWER", "REVIEWER", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":NOTE", "NOTE", OracleDbType.Varchar2));

            DbHelperOra.ExecuteSql("delete from PRO_LOG_ORIGINAL_MAP where PROCESS_ID=:PROCESS_ID and MAPID<=:MAPID",
               ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id),
               ServiceUtils.CreateOracleParameter(":MAPID", OracleDbType.Decimal, maxid));

            return true;
        }
        [WebMethod(EnableSession = true)]
        public DataSet GetData_文档类型()
        {
            return DbHelperOra.Query("SELECT DOCUMENT_TYPE_ID, DOCUMENT_TYPE FROM PKL_LOG_DOCUMENT_TYPE ORDER BY DOCUMENT_TYPE_ID");
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetData_图件类型()
        {
            return DbHelperOra.Query("select DISTINCT MAPS_CHINESE_NAME from PKL_LOG_MAP_CURVE");
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetData_解释处理项目编码()
        {
            return DbHelperOra.Query("SELECT PROCESSING_ITEM_ID, PROCESSING_ITEM_NAME,PROCESSING_ITEM_CODE FROM pkl_log_ops_project ORDER BY PROCESSING_ITEM_ID");
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetData_解释图件编码()
        {
            return DbHelperOra.Query("select MAPS_CHINESE_NAME,MAPS_CODING,PROCESSING_ITEM_ID from PKL_LOG_OPS_MAP order by rowid");
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetDataList_解释处理作业(byte[] searchModel, int page, out int total)
        {
            int pageSize = 50;
            var model = Utility.ModelHelper.DeserializeObject(searchModel) as Model.PRO_LOG_DATA_PUBLISH;
            var parameters = new List<OracleParameter>();
            var SQL = new StringBuilder();
            var whereSql = new StringBuilder("");
            SQL.Append("SELECT * ");
            SQL.Append("FROM ");
            SQL.Append("  (SELECT A.Well_Job_Name, ");
            SQL.Append("  A1.WELL_LEGAL_NAME, ");
            SQL.Append("    B.Process_Id, ");
            SQL.Append("    B.Process_Name, ");
            SQL.Append("    C.Log_Start_Time, ");
            SQL.Append("    C.Processor, ");
            SQL.Append("    D.Submit_Time, ");
            SQL.Append("    E.Approved_Time, ");
            SQL.Append("    CASE ");
            SQL.Append("      WHEN F.Flow_State = 1 ");
            SQL.Append("      THEN '等待审核' ");
            SQL.Append("      WHEN F.Flow_State = 2 ");
            SQL.Append("      THEN '归档完成' ");
            SQL.Append("      WHEN F.Flow_State = 3 ");
            SQL.Append("      THEN '审核未通过' ");
            SQL.Append("      ELSE '未归档' ");
            SQL.Append("    END State ");
            SQL.Append("  FROM Com_Job_Info A, ");
            SQL.Append("  COM_WELL_BASIC A1, ");
            SQL.Append("    Dm_Log_Process B, ");
            SQL.Append("    Pro_Log_Data_Publish C, ");
            SQL.Append("    (SELECT Obj_Id, ");
            SQL.Append("      MAX(Flow_Time)Submit_Time ");
            SQL.Append("    FROM Sys_Work_Flow ");
            SQL.Append("    WHERE Flow_Type =5 ");
            SQL.Append("    AND Flow_State  =1 ");
            SQL.Append("    GROUP BY Obj_Id ");
            SQL.Append("    ) D, ");
            SQL.Append("    (SELECT Obj_Id, ");
            SQL.Append("      MAX(Flow_Time)Approved_Time ");
            SQL.Append("    FROM Sys_Work_Flow ");
            SQL.Append("    WHERE Flow_Type =5 ");
            SQL.Append("    AND Flow_State  =2 ");
            SQL.Append("    GROUP BY Obj_Id ");
            SQL.Append("    ) E, ");
            SQL.Append("    (SELECT Obj_Id, ");
            SQL.Append("      Flow_State ");
            SQL.Append("    FROM Sys_Work_Flow_Now ");
            SQL.Append("    WHERE Flow_Type =5 ");
            SQL.Append("    ) F ");
            SQL.Append("  WHERE A.Drill_Job_Id=B.Drill_Job_Id ");
            SQL.Append("AND A.WELL_ID= A1.WELL_ID ");
            SQL.Append("  AND B.Process_Id    =C.Process_Id ");
            SQL.Append("  AND D.Obj_Id (+)    = B.Process_Id ");
            SQL.Append("  AND E.Obj_Id (+)    = B.Process_Id ");
            SQL.Append("  AND F.Obj_Id (+)    = B.Process_Id ");
            SQL.Append("  )");

            if (model != null)
            {
                if (!string.IsNullOrEmpty(model.PROCESS_ID))
                {
                    whereSql.Append(" and WELL_JOB_NAME like :WELL_JOB_NAME");
                    parameters.Add(ServiceUtils.CreateOracleParameter(":WELL_JOB_NAME", OracleDbType.Varchar2, "%" + model.PROCESS_ID + "%"));
                }

                if (!string.IsNullOrEmpty(model.INTERPRET_CENTER))
                {
                    whereSql.Append(" and STATE=:STATE");
                    parameters.Add(ServiceUtils.CreateOracleParameter(":STATE", OracleDbType.Varchar2, model.INTERPRET_CENTER));
                }

                if (!string.IsNullOrEmpty(model.PROCESSOR))
                {
                    whereSql.Append(" and PROCESSOR like :PROCESSOR");
                    parameters.Add(ServiceUtils.CreateOracleParameter(":PROCESSOR", OracleDbType.Varchar2, "%" + model.PROCESSOR + "%"));
                }

                if (model.NOTE != null && model.NOTE.ToUpper() != "ALL")
                {
                    var arr = model.NOTE.Split('|');
                    var date = Convert.ToDateTime(DbHelperOra.GetSingle("select FN_QUERY_DATE(:F,:N) from dual",
                        ServiceUtils.CreateOracleParameter(":F", OracleDbType.Varchar2, arr[0]),
                        ServiceUtils.CreateOracleParameter(":N", OracleDbType.Decimal, int.Parse(arr[1]))));
                    if (int.Parse(arr[1]) == 0)
                    {
                        whereSql.Append(" and LOG_START_TIME<:QUERY_DATE_1");
                        parameters.Add(ServiceUtils.CreateOracleParameter(":QUERY_DATE_1", OracleDbType.TimeStamp, date.AddDays(1)));
                    }
                    whereSql.Append(" and LOG_START_TIME>=:QUERY_DATE");
                    parameters.Add(ServiceUtils.CreateOracleParameter(":QUERY_DATE", OracleDbType.TimeStamp, date));
                    if (ServiceUtils.GetUserInfo().COL_LOGINNAME != "admin")
                    {
                        whereSql.Append(" and PROCESSOR=:PROCESSOR");//默认只显示当前用户的项目
                        parameters.Add(ServiceUtils.CreateOracleParameter(":PROCESSOR", OracleDbType.Varchar2, ServiceUtils.GetUserInfo().COL_NAME));
                    }

                }
                if (whereSql.Length > 0)
                {
                    whereSql.Remove(0, 4);
                    whereSql.Insert(0, "where");
                }
            }
            var finalSql = SQL.ToString() + whereSql.ToString() + " order by LOG_START_TIME DESC Nulls Last,cast(PROCESS_ID as int) DESC";
            total = Convert.ToInt32(DbHelperOra.GetSingle("select count(*) from(" + finalSql + ")", parameters.ToArray()));
            var pageSql = "select * from(select ROWNUM RN,K.* from(" + finalSql + ") K where ROWNUM<=" + page * pageSize + ") where RN>=" + ((page - 1) * pageSize + 1);
            return DbHelperOra.Query(pageSql, parameters.ToArray());
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetDefaultValue_解释处理作业(string job_plan_cd)
        {
            var ds = new DataSet();
            var dt1 = DbHelperOra.Query("select a.log_total_time,a.lost_time,b.team_org_id,b.log_series_id from dm_log_work_aning a,dm_log_up_equip b where a.job_plan_cd=b.job_plan_cd and a.job_plan_cd=:JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd)).Tables[0];
            var dt2 = DbHelperOra.Query("select logging_name,log_item_id,scale,rlev from dm_log_items where job_plan_cd=:JOB_PLAN_CD "//and down_well_sequence=(select max(down_well_sequence) from dm_log_items where job_plan_cd=:JOB_PLAN_CD)
                , ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd)).Tables[0];
            dt1.TableName = "DM_LOG_BASE";
            dt2.TableName = "DM_LOG_ITEMS";
            ds.Tables.Add(dt1.Copy());
            ds.Tables.Add(dt2.Copy());
            return ds;
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetData_解释处理作业(string process_id)
        {
            var ds = new DataSet();
            var dt1 = DbHelperOra.Query("select * from DM_LOG_PROCESS where PROCESS_ID=:PROCESS_ID", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id) }).Tables[0];
            var dt2 = DbHelperOra.Query("select * from DM_LOG_SOURCE_DATA where LOG_DATA_ID=:PROCESS_ID", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id) }).Tables[0];
            var dt3 = DbHelperOra.Query("select * from PRO_LOG_DATA_PUBLISH where PROCESS_ID=:PROCESS_ID", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id) }).Tables[0];
            var dt4 = DbHelperOra.Query("select * from PRO_LOG_PROCESSING_ITEM where PROCESS_ID=:PROCESS_ID", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id) }).Tables[0];
            var dt5 = DbHelperOra.Query("select * from PRO_LOG_PROCESS_MAP where PROCESS_ID=:PROCESS_ID", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id) }).Tables[0];
            var dt6 = DbHelperOra.Query("select * from PRO_LOG_PROCESSING_CURVERATING where PROCESS_ID=:PROCESS_ID", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id) }).Tables[0];
            var dt7 = DbHelperOra.Query("select * from PRO_LOG_ORIGINAL_MAP where PROCESS_ID=:PROCESS_ID", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id) }).Tables[0];
            dt1.TableName = "DM_LOG_PROCESS";
            dt2.TableName = "DM_LOG_SOURCE_DATA";
            dt3.TableName = "PRO_LOG_DATA_PUBLISH";
            dt4.TableName = "PRO_LOG_PROCESSING_ITEM";
            dt5.TableName = "PRO_LOG_PROCESS_MAP";
            dt6.TableName = "PRO_LOG_PROCESSING_CURVERATING";
            dt7.TableName = "PRO_LOG_ORIGINAL_MAP";
            ds.Tables.Add(dt1.Copy());
            ds.Tables.Add(dt2.Copy());
            ds.Tables.Add(dt3.Copy());
            ds.Tables.Add(dt4.Copy());
            ds.Tables.Add(dt5.Copy());
            ds.Tables.Add(dt6.Copy());
            ds.Tables.Add(dt7.Copy());
            return ds;
        }

        [WebMethod(EnableSession = true)]
        public int SaveDataList_综合解释成果数据(string PROCESS_ID, DataTable dt)
        {
            Workflow.Controller.ValidateSave<Workflow.C解释处理作业>(PROCESS_ID);

            StringBuilder strSql = new StringBuilder();
            object maxid = DbHelperOra.GetSingle("select max(RESULTID) from COM_LOG_RESULT where PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter("PROCESS_ID", OracleDbType.Varchar2, PROCESS_ID));
            strSql.Append("insert into COM_LOG_RESULT(");
            strSql.Append("RESULTID,PROCESS_ID,LAY_ID,FORMATION_NAME,START_DEPTH,END_DEPTH,VALID_THICKNESS,EXPLAIN_CONCLUSION,POROSITY_MIN_VALUE,POROSITY_MAX_VALUE,WATER_SATURATION_MIN_VALUE,WATER_SATURATION_MAX_VALUE,PENETRATE_RATE,GR_MIN_VALUE,GR_MAX_VALUE,SOUNDWAVE_MIN_VALUE,SOUNDWAVE_MAX_VALUE,DENSITY_MIN_VALUE,DENSITY_MAX_VALUE,NEUTRON_MIN_VALUE,NEUTRON_MAX_VALUE,RESISITANCE_MIN_VALUE,RESISITANCE_MAX_VALUE,QRESISITANCE_MIN_VALUE,QRESISITANCE_MAX_VALUE,SP_MIN_VALUE,SP_MAX_VALUE,RT1_MIN_VALUE,RT1_MAX_VALUE,SH_MIN_VALUE,SH_MAX_VALUE");
            strSql.Append(") values (");
            strSql.Append("RESULTID_SEQ.nextval,:PROCESS_ID,:LAY_ID,:FORMATION_NAME,:START_DEPTH,:END_DEPTH,:VALID_THICKNESS,:EXPLAIN_CONCLUSION,:POROSITY_MIN_VALUE,:POROSITY_MAX_VALUE,:WATER_SATURATION_MIN_VALUE,:WATER_SATURATION_MAX_VALUE,:PENETRATE_RATE,:GR_MIN_VALUE,:GR_MAX_VALUE,:SOUNDWAVE_MIN_VALUE,:SOUNDWAVE_MAX_VALUE,:DENSITY_MIN_VALUE,:DENSITY_MAX_VALUE,:NEUTRON_MIN_VALUE,:NEUTRON_MAX_VALUE,:RESISITANCE_MIN_VALUE,:RESISITANCE_MAX_VALUE,:QRESISITANCE_MIN_VALUE,:QRESISITANCE_MAX_VALUE,:SP_MIN_VALUE,:SP_MAX_VALUE,:RT1_MIN_VALUE,:RT1_MAX_VALUE,:SH_MIN_VALUE,:SH_MAX_VALUE");
            strSql.Append(")");
            int i = DbHelperOra.InsertDataTable(strSql.ToString(), dt,
                //ServiceUtils.CreateOracleParameter(":RESULTID", OracleDbType.Varchar2, "RESULTID") ,            
                        ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, PROCESS_ID),
                        ServiceUtils.CreateOracleParameter(":LAY_ID", "LAY_ID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":FORMATION_NAME", "FORMATION_NAME", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":START_DEPTH", "START_DEPTH", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":END_DEPTH", "END_DEPTH", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":VALID_THICKNESS", "VALID_THICKNESS", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":EXPLAIN_CONCLUSION", "EXPLAIN_CONCLUSION", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":POROSITY_MIN_VALUE", "POROSITY_MIN_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":POROSITY_MAX_VALUE", "POROSITY_MAX_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":WATER_SATURATION_MIN_VALUE", "WATER_SATURATION_MIN_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":WATER_SATURATION_MAX_VALUE", "WATER_SATURATION_MAX_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":PENETRATE_RATE", "PENETRATE_RATE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":GR_MIN_VALUE", "GR_MIN_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":GR_MAX_VALUE", "GR_MAX_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":SOUNDWAVE_MIN_VALUE", "SOUNDWAVE_MIN_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":SOUNDWAVE_MAX_VALUE", "SOUNDWAVE_MAX_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":DENSITY_MIN_VALUE", "DENSITY_MIN_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":DENSITY_MAX_VALUE", "DENSITY_MAX_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":NEUTRON_MIN_VALUE", "NEUTRON_MIN_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":NEUTRON_MAX_VALUE", "NEUTRON_MAX_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":RESISITANCE_MIN_VALUE", "RESISITANCE_MIN_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":RESISITANCE_MAX_VALUE", "RESISITANCE_MAX_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":QRESISITANCE_MIN_VALUE", "QRESISITANCE_MIN_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":QRESISITANCE_MAX_VALUE", "QRESISITANCE_MAX_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":SP_MIN_VALUE", "SP_MIN_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":SP_MAX_VALUE", "SP_MAX_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":RT1_MIN_VALUE", "RT1_MIN_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":RT1_MAX_VALUE", "RT1_MAX_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":SH_MIN_VALUE", "SH_MIN_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":SH_MAX_VALUE", "SH_MAX_VALUE", OracleDbType.Decimal)
                        );
            DbHelperOra.ExecuteSql("delete from COM_LOG_RESULT where PROCESS_ID=:PROCESS_ID and RESULTID<=:RESULTID",
                ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, PROCESS_ID),
                ServiceUtils.CreateOracleParameter(":RESULTID", OracleDbType.Decimal, maxid)
                );
            return i;
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetDataList_综合解释成果数据(string PROCESS_ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("begin ");
            strSql.Append("delete from COM_LOG_RESULT where PROCESS_ID=:PROCESS_ID and resultid not in (select max(resultid) from COM_LOG_RESULT group by start_depth,end_depth);");
            strSql.Append("end;");
            DbHelperOra.ExecuteSqlTran(strSql.ToString(), ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, PROCESS_ID));
            return DbHelperOra.Query("select distinct * from COM_LOG_RESULT where PROCESS_ID=:PROCESS_ID order by start_depth", ServiceUtils.CreateOracleParameter("PROCESS_ID", OracleDbType.Varchar2, PROCESS_ID));
        }

        [WebMethod(EnableSession = true)]
        public void SaveDataList_页岩气解释成果数据(string PROCESS_ID, DataTable dt)
        {
            Workflow.Controller.ValidateSave<Workflow.C解释处理作业>(PROCESS_ID);
            StringBuilder strSql = new StringBuilder();
            var dic = new Dictionary<string, OracleParameter[]>();
            dic.Add("delete from COM_LOG_RESULT where PROCESS_ID=:PROCESS_ID", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, PROCESS_ID) });
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dic.Add("/*" + i + "*/insert into COM_LOG_RESULT(RESULTID,PROCESS_ID,LAY_ID,FORMATION_NAME,START_DEPTH,END_DEPTH,VALID_THICKNESS,EXPLAIN_CONCLUSION,POROSITY_MIN_VALUE,POROSITY_MAX_VALUE,WATER_SATURATION_MIN_VALUE,WATER_SATURATION_MAX_VALUE,PENETRATE_RATE,GR_MIN_VALUE,GR_MAX_VALUE,SOUNDWAVE_MIN_VALUE,SOUNDWAVE_MAX_VALUE,DENSITY_MIN_VALUE,DENSITY_MAX_VALUE,NEUTRON_MIN_VALUE,NEUTRON_MAX_VALUE) values (RESULTID_SEQ.nextval,:PROCESS_ID,:LAY_ID,:FORMATION_NAME,:START_DEPTH,:END_DEPTH,:VALID_THICKNESS,:EXPLAIN_CONCLUSION,:POROSITY_MIN_VALUE,:POROSITY_MAX_VALUE,:WATER_SATURATION_MIN_VALUE,:WATER_SATURATION_MAX_VALUE,:PENETRATE_RATE,:GR_MIN_VALUE,:GR_MAX_VALUE,:SOUNDWAVE_MIN_VALUE,:SOUNDWAVE_MAX_VALUE,:DENSITY_MIN_VALUE,:DENSITY_MAX_VALUE,:NEUTRON_MIN_VALUE,:NEUTRON_MAX_VALUE)", new OracleParameter[]{        
                        ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, PROCESS_ID),
                        ServiceUtils.CreateOracleParameter(":LAY_ID", OracleDbType.Varchar2, dt.Rows[i]["LAY_ID"]),
                        ServiceUtils.CreateOracleParameter(":FORMATION_NAME", OracleDbType.Varchar2, dt.Rows[i]["FORMATION_NAME"]),
                        ServiceUtils.CreateOracleParameter(":START_DEPTH", OracleDbType.Decimal, dt.Rows[i]["START_DEPTH"]),
                        ServiceUtils.CreateOracleParameter(":END_DEPTH", OracleDbType.Decimal, dt.Rows[i]["END_DEPTH"]),
                        ServiceUtils.CreateOracleParameter(":VALID_THICKNESS", OracleDbType.Decimal, dt.Rows[i]["VALID_THICKNESS"]),
                        ServiceUtils.CreateOracleParameter(":EXPLAIN_CONCLUSION", OracleDbType.Varchar2, dt.Rows[i]["EXPLAIN_CONCLUSION"]),
                        ServiceUtils.CreateOracleParameter(":POROSITY_MIN_VALUE", OracleDbType.Decimal, dt.Rows[i]["POROSITY_MIN_VALUE"]),
                        ServiceUtils.CreateOracleParameter(":POROSITY_MAX_VALUE", OracleDbType.Decimal, dt.Rows[i]["POROSITY_MAX_VALUE"]),
                        ServiceUtils.CreateOracleParameter(":WATER_SATURATION_MIN_VALUE", OracleDbType.Decimal, dt.Rows[i]["WATER_SATURATION_MIN_VALUE"]),
                        ServiceUtils.CreateOracleParameter(":WATER_SATURATION_MAX_VALUE", OracleDbType.Decimal, dt.Rows[i]["WATER_SATURATION_MAX_VALUE"]),
                        ServiceUtils.CreateOracleParameter(":PENETRATE_RATE", OracleDbType.Decimal, dt.Rows[i]["PENETRATE_RATE"]),
                        ServiceUtils.CreateOracleParameter(":GR_MIN_VALUE", OracleDbType.Decimal, dt.Rows[i]["GR_MIN_VALUE"]),
                        ServiceUtils.CreateOracleParameter(":GR_MAX_VALUE", OracleDbType.Decimal, dt.Rows[i]["GR_MAX_VALUE"]),
                        ServiceUtils.CreateOracleParameter(":SOUNDWAVE_MIN_VALUE", OracleDbType.Decimal, dt.Rows[i]["SOUNDWAVE_MIN_VALUE"]),
                        ServiceUtils.CreateOracleParameter(":SOUNDWAVE_MAX_VALUE", OracleDbType.Decimal, dt.Rows[i]["SOUNDWAVE_MAX_VALUE"]),
                        ServiceUtils.CreateOracleParameter(":DENSITY_MIN_VALUE", OracleDbType.Decimal, dt.Rows[i]["DENSITY_MIN_VALUE"]),
                        ServiceUtils.CreateOracleParameter(":DENSITY_MAX_VALUE", OracleDbType.Decimal, dt.Rows[i]["DENSITY_MAX_VALUE"]),
                        ServiceUtils.CreateOracleParameter(":NEUTRON_MIN_VALUE", OracleDbType.Decimal, dt.Rows[i]["NEUTRON_MIN_VALUE"]),
                        ServiceUtils.CreateOracleParameter(":NEUTRON_MAX_VALUE", OracleDbType.Decimal, dt.Rows[i]["NEUTRON_MAX_VALUE"])});

                    var organic_carbon_conten_min = dt.Rows[i]["ORGANIC_CARBON_CONTEN_MIN"];
                    var organic_carbon_conten_max = dt.Rows[i]["ORGANIC_CARBON_CONTEN_MAX"];
                    var all_gas_content_min = dt.Rows[i]["ALL_GAS_CONTENT_MIN"];
                    var all_gas_content_max = dt.Rows[i]["ALL_GAS_CONTENT_MAX"];
                    var adsorbed_gas_contnet_min = dt.Rows[i]["ADSORBED_GAS_CONTENT_MIN"];
                    var adsorbed_gas_contnet_max = dt.Rows[i]["ADSORBED_GAS_CONTENT_MAX"];
                    var free_gas_conent_min = dt.Rows[i]["FREE_GAS_CONTENT_MIN"];
                    var free_gas_conent_max = dt.Rows[i]["FREE_GAS_CONTENT_MAX"];
                    var brittleness_index_min = dt.Rows[i]["BRITTLENESS_INDEX_MIN"];
                    var brittleness_index_max = dt.Rows[i]["BRITTLENESS_INDEX_MAX"];
                    var kerogen_min = dt.Rows[i]["KEROGEN_MIN"];
                    var kerogen_max = dt.Rows[i]["KEROGEN_MAX"];
                    var induction_deep_min = dt.Rows[i]["INDUCTION_DEEP_MIN"];
                    var induction_deep_max = dt.Rows[i]["INDUCTION_DEEP_MAX"];
                    var induction_medium_min = dt.Rows[i]["INDUCTION_MEDIUM_MIN"];
                    var induction_medium_max = dt.Rows[i]["INDUCTION_MEDIUM_MAX"];
                    var induction_shallow_min = dt.Rows[i]["INDUCTION_SHALLOW_MIN"];
                    var induction_shallow_max = dt.Rows[i]["INDUCTION_SHALLOW_MAX"];
                    if (organic_carbon_conten_min != DBNull.Value || organic_carbon_conten_max != DBNull.Value ||
                        all_gas_content_min != DBNull.Value || all_gas_content_max != DBNull.Value ||
                        adsorbed_gas_contnet_min != DBNull.Value || adsorbed_gas_contnet_max != DBNull.Value ||
                        free_gas_conent_min != DBNull.Value || free_gas_conent_max != DBNull.Value ||
                        brittleness_index_min != DBNull.Value || brittleness_index_max != DBNull.Value ||
                        kerogen_min != DBNull.Value || kerogen_max != DBNull.Value ||
                        induction_deep_min != DBNull.Value || induction_deep_max != DBNull.Value ||
                        induction_medium_min != DBNull.Value || induction_medium_max != DBNull.Value ||
                        induction_shallow_min != DBNull.Value || induction_shallow_max != DBNull.Value
                        )
                    {
                        dic.Add("/*" + i + "*/insert into WL_ACH_SHALE_GAS_INTER(SHALE_GAS_INTER_ID,RESULTID,ORGANIC_CARBON_CONTEN_MIN,ORGANIC_CARBON_CONTEN_MAX,ALL_GAS_CONTENT_MIN,ALL_GAS_CONTENT_MAX,ADSORBED_GAS_CONTENT_MIN,ADSORBED_GAS_CONTENT_MAX,FREE_GAS_CONTENT_MIN,FREE_GAS_CONTENT_MAX,BRITTLENESS_INDEX_MIN,BRITTLENESS_INDEX_MAX,KEROGEN_MIN,KEROGEN_MAX,INDUCTION_DEEP_MIN,INDUCTION_DEEP_MAX,INDUCTION_MEDIUM_MIN,INDUCTION_MEDIUM_MAX,INDUCTION_SHALLOW_MIN,INDUCTION_SHALLOW_MAX) values (SHALE_GAS_INTER_ID_SEQ.nextval,RESULTID_SEQ.currval,:ORGANIC_CARBON_CONTEN_MIN,:ORGANIC_CARBON_CONTEN_MAX,:ALL_GAS_CONTENT_MIN,:ALL_GAS_CONTENT_MAX,:ADSORBED_GAS_CONTENT_MIN,:ADSORBED_GAS_CONTENT_MAX,:FREE_GAS_CONTENT_MIN,:FREE_GAS_CONTENT_MAX,:BRITTLENESS_INDEX_MIN,:BRITTLENESS_INDEX_MAX,:KEROGEN_MIN,:KEROGEN_MAX,:INDUCTION_DEEP_MIN,:INDUCTION_DEEP_MAX,:INDUCTION_MEDIUM_MIN,:INDUCTION_MEDIUM_MAX,:INDUCTION_SHALLOW_MIN,:INDUCTION_SHALLOW_MAX)", new OracleParameter[] {
			            //ServiceUtils.CreateOracleParameter(":SHALE_GAS_INTER_ID", OracleDbType.Varchar2, dt.Rows[i]["SHALE_GAS_INTER_ID"]) ,            
                        //ServiceUtils.CreateOracleParameter(":RESULTID", OracleDbType.Decimal, dt.Rows[i]["RESULTID"]) ,            
                        ServiceUtils.CreateOracleParameter(":ORGANIC_CARBON_CONTEN_MIN", OracleDbType.Decimal, organic_carbon_conten_min) , 
                        ServiceUtils.CreateOracleParameter(":ORGANIC_CARBON_CONTEN_MAX", OracleDbType.Decimal, organic_carbon_conten_max) , 
                        ServiceUtils.CreateOracleParameter(":ALL_GAS_CONTENT_MIN", OracleDbType.Decimal, all_gas_content_min) ,            
                        ServiceUtils.CreateOracleParameter(":ALL_GAS_CONTENT_MAX", OracleDbType.Decimal, all_gas_content_max) ,  
                        ServiceUtils.CreateOracleParameter(":ADSORBED_GAS_CONTENT_MIN", OracleDbType.Decimal, adsorbed_gas_contnet_min) , 
                        ServiceUtils.CreateOracleParameter(":ADSORBED_GAS_CONTENT_MAX", OracleDbType.Decimal, adsorbed_gas_contnet_max) , 
                        ServiceUtils.CreateOracleParameter(":FREE_GAS_CONTENT_MIN", OracleDbType.Decimal, free_gas_conent_min) ,            
                        ServiceUtils.CreateOracleParameter(":FREE_GAS_CONTENT_MAX", OracleDbType.Decimal, free_gas_conent_max) , 
                        ServiceUtils.CreateOracleParameter(":BRITTLENESS_INDEX_MIN", OracleDbType.Decimal, brittleness_index_min) ,  
                        ServiceUtils.CreateOracleParameter(":BRITTLENESS_INDEX_MAX", OracleDbType.Decimal, brittleness_index_max) , 
                        ServiceUtils.CreateOracleParameter(":KEROGEN_MIN", OracleDbType.Decimal, kerogen_min) , 
                        ServiceUtils.CreateOracleParameter(":KEROGEN_MAX", OracleDbType.Decimal, kerogen_max) , 
                        ServiceUtils.CreateOracleParameter(":INDUCTION_DEEP_MIN", OracleDbType.Decimal, induction_deep_min) , 
                        ServiceUtils.CreateOracleParameter(":INDUCTION_DEEP_MAX", OracleDbType.Decimal, induction_deep_max) ,
                        ServiceUtils.CreateOracleParameter(":INDUCTION_MEDIUM_MIN", OracleDbType.Decimal, induction_medium_min) ,            
                        ServiceUtils.CreateOracleParameter(":INDUCTION_MEDIUM_MAX", OracleDbType.Decimal, induction_medium_max) , 
                        ServiceUtils.CreateOracleParameter(":INDUCTION_SHALLOW_MIN", OracleDbType.Decimal, induction_shallow_min),
                        ServiceUtils.CreateOracleParameter(":INDUCTION_SHALLOW_MAX", OracleDbType.Decimal, induction_shallow_max)});
                    }
                }
            }
            DbHelperOra.ExecuteSqlTran(dic);
            //return i;
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetDataList_页岩气解释成果数据(string PROCESS_ID)
        {
            return DbHelperOra.Query("select * from COM_LOG_RESULT A,WL_ACH_SHALE_GAS_INTER B where PROCESS_ID=:PROCESS_ID and A.RESULTID = B.RESULTID(+) order by A.RESULTID", ServiceUtils.CreateOracleParameter("PROCESS_ID", OracleDbType.Varchar2, PROCESS_ID));
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetCurveNames(decimal item_id)
        {
            //return DbHelperOra.Query("select curve_name,max(curve_cd) curve_cd,min(curve_id) id from pkl_log_ops_curve group by curve_name order by id");
            return DbHelperOra.Query("SELECT CURVE_NAME,CURVE_CD FROM PKL_LOG_OPS_CURVE WHERE PROCESSING_ITEM_ID = " + item_id + " ORDER BY CURVE_ID");
        }
        [WebMethod(EnableSession = true)]
        public DataSet GetMapCurveInfo(string map_name)
        {
            //return DbHelperOra.Query("select curve_name,max(curve_cd) curve_cd,min(curve_id) id from pkl_log_ops_curve group by curve_name order by id");
            return DbHelperOra.Query("SELECT A.MAP_CURVE_ID,A.MAPS_CHINESE_NAME,B.PROCESSING_ITEM_ID,B.CURVE_CD,B.CURVE_NAME FROM PKL_LOG_MAP_CURVE A,PKL_LOG_OPS_CURVE B WHERE A.CURVE_ID=B.CURVE_ID(+) AND A.MAPS_CHINESE_NAME=:MAPS_CHINESE_NAME ORDER BY A.CURVE_ID",
                ServiceUtils.CreateOracleParameter(":MAPS_CHINESE_NAME", OracleDbType.Varchar2, map_name));
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetAllCurveNames()
        {
            //return DbHelperOra.Query("select curve_name,max(curve_cd) curve_cd,min(curve_id) id from pkl_log_ops_curve group by curve_name order by id");
            return DbHelperOra.Query("SELECT CURVE_ID,PROCESSING_ITEM_ID,CURVE_NAME,CURVE_CD FROM PKL_LOG_OPS_CURVE ORDER BY CURVE_ID");
        }

        [WebMethod(EnableSession = true)]
        public int SaveDataList_固井质量评价数据(string PROCESS_ID, DataTable dt)
        {
            Workflow.Controller.ValidateSave<Workflow.C解释处理作业>(PROCESS_ID);

            StringBuilder strSql = new StringBuilder();
            object maxid = DbHelperOra.GetSingle("select max(CEMENTID) from COM_LOG_CEMENT_EVALUATION_INF where PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter("PROCESS_ID", OracleDbType.Varchar2, PROCESS_ID));
            strSql.Append("insert into COM_LOG_CEMENT_EVALUATION_INF(");
            strSql.Append("CEMENTID,PROCESS_ID,ST_DEP,EN_DEP,MAX_CBL,MIN_CBL,MEA_CBL,RESULT");
            strSql.Append(") values (");
            strSql.Append("CEMENTID_SEQ.nextval,:PROCESS_ID,:ST_DEP,:EN_DEP,:MAX_CBL,:MIN_CBL,:MEA_CBL,:RESULT");
            strSql.Append(") ");
            int i = DbHelperOra.InsertDataTable(strSql.ToString(), dt,
                        ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, PROCESS_ID),
                        ServiceUtils.CreateOracleParameter(":ST_DEP", "ST_DEP", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":EN_DEP", "EN_DEP", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":MAX_CBL", "MAX_CBL", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":MIN_CBL", "MIN_CBL", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":MEA_CBL", "MEA_CBL", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":RESULT", "RESULT", OracleDbType.Char)
                        );
            DbHelperOra.ExecuteSql("delete from COM_LOG_CEMENT_EVALUATION_INF where PROCESS_ID=:PROCESS_ID and CEMENTID<=:CEMENTID",
                ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, PROCESS_ID),
                ServiceUtils.CreateOracleParameter(":CEMENTID", OracleDbType.Decimal, maxid)
                );
            return i;
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetDataList_固井质量评价数据(string PROCESS_ID)
        {
            return DbHelperOra.Query("select * from COM_LOG_CEMENT_EVALUATION_INF where PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter("PROCESS_ID", OracleDbType.Varchar2, PROCESS_ID));
        }

        [WebMethod(EnableSession = true)]
        public int SaveDataList_测量井斜数据(string PROCESS_ID, DataTable dt)
        {
            Workflow.Controller.ValidateSave<Workflow.C解释处理作业>(PROCESS_ID);

            StringBuilder strSql = new StringBuilder();
            object maxid = DbHelperOra.GetSingle("select max(DEV_ID) from COM_LOG_DEV_AZI where PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter("PROCESS_ID", OracleDbType.Varchar2, PROCESS_ID));
            strSql.Append("insert into COM_LOG_DEV_AZI(");
            strSql.Append("DEV_ID,PROCESS_ID,DEP,INCLNATION,AZIMUTH");
            strSql.Append(") values (");
            strSql.Append("DEV_ID_SEQ.nextval,:PROCESS_ID,:DEP,:INCLNATION,:AZIMUTH");
            strSql.Append(") ");
            int i = DbHelperOra.InsertDataTable(strSql.ToString(), dt,
                        ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, PROCESS_ID),
                        ServiceUtils.CreateOracleParameter(":DEP", "DEP", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":INCLNATION", "INCLNATION", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":AZIMUTH", "AZIMUTH", OracleDbType.Decimal)
                        );
            DbHelperOra.ExecuteSql("delete from COM_LOG_DEV_AZI where PROCESS_ID=:PROCESS_ID and DEV_ID<=:DEV_ID",
                ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, PROCESS_ID),
                ServiceUtils.CreateOracleParameter(":DEV_ID", OracleDbType.Decimal, maxid)
                );
            return i;
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetDataList_测量井斜数据(string PROCESS_ID)
        {
            return DbHelperOra.Query("select * from COM_LOG_DEV_AZI where PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter("PROCESS_ID", OracleDbType.Varchar2, PROCESS_ID));
        }

        [WebMethod(EnableSession = true)]
        public int SaveDataList_综合解释成果曲线数据(string PROCESS_ID, DataTable dt)
        {
            Workflow.Controller.ValidateSave<Workflow.C解释处理作业>(PROCESS_ID);

            StringBuilder strSql = new StringBuilder();
            object maxid = DbHelperOra.GetSingle("select max(CURVEDATAID) from COM_LOG_COM_CURVEDATA where PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter("PROCESS_ID", OracleDbType.Varchar2, PROCESS_ID));
            strSql.Append("insert into COM_LOG_COM_CURVEDATA(");
            strSql.Append("CURVEDATAID,PROCESS_ID,DEP,GR,CAL,DEV,DAZ,AC,CNL,DEN,PE,RT,RXO,U,TH,K");
            strSql.Append(") values (");
            strSql.Append("CURVEDATAID_SEQ.nextval,:PROCESS_ID,:DEP,:GR,:CAL,:DEV,:DAZ,:AC,:CNL,:DEN,:PE,:RT,:RXO,:U,:TH,:K");
            strSql.Append(") ");

            int i = DbHelperOra.InsertDataTable(strSql.ToString(), dt,
                        ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, PROCESS_ID),
                        ServiceUtils.CreateOracleParameter(":DEP", "DEP", OracleDbType.Decimal),
                //ServiceUtils.CreateOracleParameter(":SP","SP", OracleDbType.Decimal) ,            
                        ServiceUtils.CreateOracleParameter(":GR", "GR", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CAL", "CAL", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":DEV", "DEV", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":DAZ", "DAZ", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":AC", "AC", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CNL", "CNL", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":DEN", "DEN", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":PE", "PE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":RT", "RT", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":RXO", "RXO", OracleDbType.Decimal),
                //ServiceUtils.CreateOracleParameter(":SGR","SGR", OracleDbType.Decimal) ,            
                //ServiceUtils.CreateOracleParameter(":CGR","CGR", OracleDbType.Decimal) ,            
                        ServiceUtils.CreateOracleParameter(":U", "U", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":TH", "TH", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":K", "K", OracleDbType.Decimal)
                        );
            DbHelperOra.ExecuteSql("delete from COM_LOG_COM_CURVEDATA where PROCESS_ID=:PROCESS_ID and CURVEDATAID<=:CURVEDATAID",
                ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, PROCESS_ID),
                ServiceUtils.CreateOracleParameter(":CURVEDATAID", OracleDbType.Decimal, maxid)
                );
            return i;
        }

        [WebMethod(EnableSession = true)]
        public decimal? SaveCurveDataCache(string process_id, string md5, string sha1, long length, byte[] comboBoxsSetting)
        {
            decimal? uploadid = null;
            if (string.IsNullOrWhiteSpace(process_id)) return uploadid;

            if (!string.IsNullOrWhiteSpace(md5))
            {
                uploadid = DbHelperOra.GetSingle("select uploadid from SYS_UPLOAD where sha1=:SHA1 and md5=:MD5 and length=:LENGTH",
                ServiceUtils.CreateOracleParameter(":SHA1", OracleDbType.Varchar2, sha1),
                ServiceUtils.CreateOracleParameter(":MD5", OracleDbType.Varchar2, md5),
                ServiceUtils.CreateOracleParameter(":LENGTH", OracleDbType.Decimal, length)
                ) as decimal?;
            }
            DbHelperOra.ExecuteSqlTran("DECLARE v_count number;BEGIN select count(1) into v_count from com_log_com_curvedata_cache where process_id=:PROCESS_ID;IF v_count=0 THEN INSERT INTO com_log_com_curvedata_cache(process_id) VALUES(:PROCESS_ID);END IF ;update com_log_com_curvedata_cache set uploadid=:UPLOADID,comboboxs_setting=:SEETING where process_id=:PROCESS_ID;END;",
                ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id),
                ServiceUtils.CreateOracleParameter(":UPLOADID", OracleDbType.Decimal, uploadid),
                ServiceUtils.CreateOracleParameter(":SEETING", OracleDbType.Blob, comboBoxsSetting));
            return uploadid;
        }

        [WebMethod(EnableSession = true)]
        public void TransOldComCurveData()
        {
            var cache_dt = DbHelperOra.Query("select process_id,uploadid from com_log_com_curvedata_cache").Tables[0];
            if (cache_dt.Rows.Count > 0)
            {
                for (int i = 0; i < cache_dt.Rows.Count; i++)
                {
                    var uploadid = cache_dt.Rows[i].FieldEx<decimal>("uploadid");
                    var process_id = cache_dt.Rows[i].FieldEx<string>("process_id");
                    var upload_dt = DbHelperOra.Query("select * from sys_upload where uploadid=:uploadid",
                        ServiceUtils.CreateOracleParameter(":uploadid", OracleDbType.Decimal, uploadid)).Tables[0];
                    var sha1 = upload_dt.Rows[0].FieldEx<string>("SHA1");
                    if (sha1 == null) continue;
                    var filePath = string.Format("{0}\\{1}-{2}-{3}", @"I:\cejing\LoggingApp\fileupload\201610", sha1, upload_dt.Rows[0].FieldEx<string>("MD5"), upload_dt.Rows[0].FieldEx<decimal>("LENGTH"));
                    if (!File.Exists(filePath))
                    {
                        File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "LogFile\\cg_old_err.txt", DateTime.Now + "--未找到文件:" + filePath + "\r\n");
                        continue;
                    }
                    var combosetting = DbHelperOra.GetSingle("select comboboxs_setting from com_log_com_curvedata_cache where process_id=:processid and uploadid=:uploadid",
                        ServiceUtils.CreateOracleParameter(":processid", OracleDbType.Varchar2, process_id),
                        ServiceUtils.CreateOracleParameter(":uploadid", OracleDbType.Decimal, uploadid));
                    var curveHelper = new CurveDataHelper();
                    var curvedt = curveHelper.GetComLogCurveDT(filePath, (byte[])combosetting);
                    try
                    {
                        SaveCurveData(curvedt, process_id);
                    }
                    catch (Exception ex)
                    {
                        File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "LogFile\\cg_old_err.txt", DateTime.Now + "--文件:" + filePath + "," + ex.Message + "\r\n");
                    }
                }
                File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "LogFile\\cg_old_err.txt", DateTime.Now + "--转化完成.\r\n");
            }
        }

        [WebMethod(EnableSession = true)]
        public void TransOldProcessingCurveData()
        {
            var process_dt = DbHelperOra.Query("select process_id,processing_item_id from pro_log_processing_curve group by process_id,processing_item_id").Tables[0];
            if (process_dt.Rows.Count > 0)
            {
                for (int i = 0; i < process_dt.Rows.Count; i++)
                {
                    var processid = process_dt.Rows[i].FieldEx<string>("process_id");
                    var itemid = process_dt.Rows[i].FieldEx<decimal>("processing_item_id");
                    var dt = DbHelperOra.Query("select A.*,C.* from pro_log_processing_curve A,sys_file_upload B,sys_upload C where A.FILEID=B.FILEID(+) and B.UPLOADID=C.UPLOADID(+) and A.process_id=:process_id and A.processing_item_id=:item_id",
                             ServiceUtils.CreateOracleParameter(":process_id", OracleDbType.Varchar2, processid),
                             ServiceUtils.CreateOracleParameter(":item_id", OracleDbType.Decimal, itemid)).Tables[0];
                    var curveHelper = new CurveDataHelper();
                    var curvedt = curveHelper.GetProcessingCurveDT(dt, @"I:\cejing\LoggingApp\fileupload\201610");
                    try
                    {
                        SaveCurveInfo(processid, itemid, curvedt);
                    }
                    catch (Exception ex)
                    {
                        File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "LogFile\\tj_old_err.txt", DateTime.Now + ":" + ex.Message + "\r\n");
                    }
                }
                File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "LogFile\\tj_old_err.txt", DateTime.Now + "--转化完成.\r\n");
            }
        }


        [WebMethod(EnableSession = true)]
        public DataSet GetCurveDataCache(string process_id)
        {
            return DbHelperOra.Query("select uploadid,comboboxs_setting from com_log_com_curvedata_cache where process_id=:PROCESS_ID"
                , ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id));
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetCurveData(string PROCESS_ID)
        {
            return DbHelperOra.Query("select A.CURVE_CD,A.CURVE_START_DEP,A.CURVE_END_DEP,A.CURVE_RLEV,A.CURVE_SAMPLE,B.BLOCK_NUMBER,B.BLOCK_SIZE,B.CURVE_DATA from COM_LOG_COM_CURVE_INDEX A,COM_LOG_COM_CURVE_DATA B where A.PROCESS_ID=:PROCESS_ID and A.CURVEID=B.CURVEID", ServiceUtils.CreateOracleParameter("PROCESS_ID", OracleDbType.Varchar2, PROCESS_ID));
        }

        [WebMethod(EnableSession = true)]
        public int SaveCurveData(DataTable dt, string PROCESS_ID)
        {
            Workflow.Controller.ValidateSave<Workflow.C解释处理作业>(PROCESS_ID);

            object process_name = DbHelperOra.GetSingle("select PROCESS_NAME from DM_LOG_PROCESS where PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter("PROCESS_ID", OracleDbType.Varchar2, PROCESS_ID));
            string process_item_code = null;
            object process_item_id = null;
            if (process_name != null)
            {
                if (process_name.ToString().Contains("#"))
                    process_item_code = process_name.ToString().Split('#')[1].Split('_')[0];
            }

            if (process_item_code != null)
            {
                process_item_id = DbHelperOra.GetSingle("select PROCESSING_ITEM_ID from PKL_LOG_OPS_PROJECT where PROCESSING_ITEM_CODE like '%" + process_item_code + "%'");
            }

            object maxid = DbHelperOra.GetSingle("select max(CURVEID) from COM_LOG_COM_CURVE_INDEX where PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter("PROCESS_ID", OracleDbType.Varchar2, PROCESS_ID));
            object interpret_center = DbHelperOra.GetSingle("select INTERPRET_CENTER from PRO_LOG_DATA_PUBLISH where PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter("PROCESS_ID", OracleDbType.Varchar2, PROCESS_ID));
            StringBuilder strSql = new StringBuilder();
            strSql.Append("declare V_CURVEID nvarchar2(32);");
            strSql.Append("begin ");
            strSql.Append("select CURVEID_SEQ_1.nextval into V_CURVEID from dual;");
            strSql.Append("insert into COM_LOG_COM_CURVE_INDEX(");
            strSql.Append("CURVEID,PROCESS_ID,PROCESSING_ITEM_ID,CURVE_NAME,CURVE_CD,CURVE_START_DEP,CURVE_END_DEP,CURVE_RLEV,CURVE_UNIT,CURVE_SAMPLE,CURVE_MAX_VALUE,CURVE_MIN_VALUE,CURVE_DATA_TYPE,CURVE_DATA_LENGTH,P_CURVESOFTWARE_NAME,DATA_SIZE,CURVE_NOTE,NOTE");
            strSql.Append(") values (");
            strSql.Append("V_CURVEID,:PROCESS_ID,:PROCESSING_ITEM_ID,:CURVE_NAME,:CURVE_CD,:CURVE_START_DEP,:CURVE_END_DEP,:CURVE_RLEV,:CURVE_UNIT,:CURVE_SAMPLE,:CURVE_MAX_VALUE,:CURVE_MIN_VALUE,:CURVE_DATA_TYPE,:CURVE_DATA_LENGTH,:P_CURVESOFTWARE_NAME,:DATA_SIZE,:CURVE_NOTE,:NOTE");
            strSql.Append(");");
            strSql.Append("insert into COM_LOG_COM_CURVE_DATA(CURVEID,BLOCK_NUMBER,BLOCK_SIZE,CURVE_DATA) values (V_CURVEID,:BLOCK_NUMBER,:BLOCK_SIZE,:CURVE_DATA); ");
            strSql.Append("end;");

            int i = DbHelperOra.InsertDataTable(strSql.ToString(), dt,
                        ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, PROCESS_ID),
                        ServiceUtils.CreateOracleParameter(":PROCESSING_ITEM_ID", OracleDbType.Decimal, process_item_id),
                        ServiceUtils.CreateOracleParameter(":CURVE_NAME", "CURVE_NAME", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":CURVE_CD", "CURVE_CD", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":CURVE_START_DEP", "CURVE_START_DEP", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURVE_END_DEP", "CURVE_END_DEP", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURVE_RLEV", "CURVE_RLEV", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURVE_UNIT", "CURVE_UNIT", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":CURVE_SAMPLE", "CURVE_SAMPLE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURVE_MAX_VALUE", "CURVE_MAX_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURVE_MIN_VALUE", "CURVE_MIN_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURVE_DATA_TYPE", "CURVE_DATA_TYPE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":CURVE_DATA_LENGTH", "CURVE_DATA_LENGTH", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":P_CURVESOFTWARE_NAME", OracleDbType.Varchar2, interpret_center),
                        ServiceUtils.CreateOracleParameter(":DATA_SIZE", "DATA_SIZE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURVE_NOTE", "CURVE_NOTE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":NOTE", "NOTE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":BLOCK_NUMBER", "BLOCK_NUMBER", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":BLOCK_SIZE", "BLOCK_SIZE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURVE_DATA", "CURVE_DATA", OracleDbType.Blob)
                        );
            DbHelperOra.ExecuteSql("delete from COM_LOG_COM_CURVE_INDEX where PROCESS_ID=:PROCESS_ID and CURVEID<=:CURVEID",
            ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, PROCESS_ID),
            ServiceUtils.CreateOracleParameter(":CURVEID", OracleDbType.Decimal, maxid)
            );
            return i;
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetThreePressureData(string PROCESS_ID)
        {
            return DbHelperOra.Query("select A.CURVE_CD,A.CURVE_START_DEP,A.CURVE_END_DEP,A.CURVE_RLEV,A.CURVE_SAMPLE,B.BLOCK_NUMBER,B.BLOCK_SIZE,B.CURVE_DATA from PRO_LOG_THRESS_PRESSURE_INDEX A,PRO_LOG_THRESS_PRESSURE_DATA B where A.PROCESS_ID=:PROCESS_ID and A.CURVEID=B.CURVEID", ServiceUtils.CreateOracleParameter("PROCESS_ID", OracleDbType.Varchar2, PROCESS_ID));
        }

        [WebMethod(EnableSession = true)]
        public int SaveThreePressureData(DataTable dt, string PROCESS_ID)
        {
            Workflow.Controller.ValidateSave<Workflow.C解释处理作业>(PROCESS_ID);

            object process_name = DbHelperOra.GetSingle("select PROCESS_NAME from DM_LOG_PROCESS where PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter("PROCESS_ID", OracleDbType.Varchar2, PROCESS_ID));
            string process_item_code = null;
            object process_item_id = null;
            if (process_name != null)
            {
                if (process_name.ToString().Contains("#"))
                    process_item_code = process_name.ToString().Split('#')[1].Split('_')[0];
            }

            if (process_item_code != null)
            {
                process_item_id = DbHelperOra.GetSingle("select PROCESSING_ITEM_ID from PKL_LOG_OPS_PROJECT where PROCESSING_ITEM_CODE like '%" + process_item_code + "%'");
            }

            object maxid = DbHelperOra.GetSingle("select max(CURVEID) from PRO_LOG_THRESS_PRESSURE_INDEX where PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter("PROCESS_ID", OracleDbType.Varchar2, PROCESS_ID));
            object interpret_center = DbHelperOra.GetSingle("select INTERPRET_CENTER from PRO_LOG_DATA_PUBLISH where PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter("PROCESS_ID", OracleDbType.Varchar2, PROCESS_ID));
            StringBuilder strSql = new StringBuilder();
            strSql.Append("declare V_CURVEID nvarchar2(32);");
            strSql.Append("begin ");
            strSql.Append("select CURVEID_SEQ_3.nextval into V_CURVEID from dual;");
            strSql.Append("insert into PRO_LOG_THRESS_PRESSURE_INDEX(");
            strSql.Append("CURVEID,PROCESS_ID,PROCESSING_ITEM_ID,CURVE_NAME,CURVE_CD,CURVE_START_DEP,CURVE_END_DEP,CURVE_RLEV,CURVE_UNIT,CURVE_SAMPLE,CURVE_MAX_VALUE,CURVE_MIN_VALUE,CURVE_DATA_TYPE,CURVE_DATA_LENGTH,P_CURVESOFTWARE_NAME,DATA_SIZE,CURVE_NOTE,NOTE");
            strSql.Append(") values (");
            strSql.Append("V_CURVEID,:PROCESS_ID,:PROCESSING_ITEM_ID,:CURVE_NAME,:CURVE_CD,:CURVE_START_DEP,:CURVE_END_DEP,:CURVE_RLEV,:CURVE_UNIT,:CURVE_SAMPLE,:CURVE_MAX_VALUE,:CURVE_MIN_VALUE,:CURVE_DATA_TYPE,:CURVE_DATA_LENGTH,:P_CURVESOFTWARE_NAME,:DATA_SIZE,:CURVE_NOTE,:NOTE");
            strSql.Append(");");
            strSql.Append("insert into PRO_LOG_THRESS_PRESSURE_DATA(CURVEID,BLOCK_NUMBER,BLOCK_SIZE,CURVE_DATA) values (V_CURVEID,:BLOCK_NUMBER,:BLOCK_SIZE,:CURVE_DATA); ");
            strSql.Append("end;");

            int i = DbHelperOra.InsertDataTable(strSql.ToString(), dt,
                        ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, PROCESS_ID),
                        ServiceUtils.CreateOracleParameter(":PROCESSING_ITEM_ID", OracleDbType.Decimal, process_item_id),
                        ServiceUtils.CreateOracleParameter(":CURVE_NAME", "CURVE_NAME", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":CURVE_CD", "CURVE_CD", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":CURVE_START_DEP", "CURVE_START_DEP", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURVE_END_DEP", "CURVE_END_DEP", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURVE_RLEV", "CURVE_RLEV", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURVE_UNIT", "CURVE_UNIT", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":CURVE_SAMPLE", "CURVE_SAMPLE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURVE_MAX_VALUE", "CURVE_MAX_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURVE_MIN_VALUE", "CURVE_MIN_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURVE_DATA_TYPE", "CURVE_DATA_TYPE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":CURVE_DATA_LENGTH", "CURVE_DATA_LENGTH", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":P_CURVESOFTWARE_NAME", OracleDbType.Varchar2, interpret_center),
                        ServiceUtils.CreateOracleParameter(":DATA_SIZE", "DATA_SIZE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURVE_NOTE", "CURVE_NOTE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":NOTE", "NOTE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":BLOCK_NUMBER", "BLOCK_NUMBER", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":BLOCK_SIZE", "BLOCK_SIZE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURVE_DATA", "CURVE_DATA", OracleDbType.Blob)
                        );
            DbHelperOra.ExecuteSql("delete from PRO_LOG_THRESS_PRESSURE_INDEX where PROCESS_ID=:PROCESS_ID and CURVEID<=:CURVEID",
            ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, PROCESS_ID),
            ServiceUtils.CreateOracleParameter(":CURVEID", OracleDbType.Decimal, maxid)
            );
            return i;
        }


        [WebMethod(EnableSession = true)]
        public List<string> GetCurveDataCachePage(decimal uploadid, int startLine, int page, out int countLine)
        {
            countLine = 0;
            if (startLine < 1) return null;
            var dt = DbHelperOra.Query("select sha1,md5,length,pathid,PATHMAIN from sys_upload where uploadid =" + uploadid).Tables[0];
            if (dt == null || dt.Rows.Count == 0) return null;
            var txtList = new List<string>();

            var path = FileUpload.PathList.FirstOrDefault(o => o.id == dt.Rows[0].FieldEx<decimal>("PATHID")).name;
            path = path.Remove(path.LastIndexOf('\\') + 1);
            path += dt.Rows[0].FieldEx<string>("PATHMAIN");

            string fullname = string.Format("{0}\\{1}-{2}-{3}", path,
                dt.Rows[0].FieldEx<string>("SHA1"),
                dt.Rows[0].FieldEx<string>("MD5"),
                dt.Rows[0].FieldEx<decimal>("LENGTH"));
            if (!File.Exists(fullname)) return null;
            int pageSize = 50;
            int beginLine = 50 * (page - 1) + startLine;
            var reader = new StreamReader(fullname, Encoding.Default, true);
            int num = 1;
            if (beginLine > 1000 && reader.BaseStream.Length > 1024 * 1024 * 10)
            {
                var list = DataCache.GetFileLineIndex(fullname);
                var index = list.FirstOrDefault(o => o.StartLine <= beginLine && o.EndLine >= beginLine);
                if (index.StartLine > 0)
                {
                    reader.BaseStream.Seek(index.Position, SeekOrigin.Begin);
                    num = index.StartLine;
                }
                countLine = list.Max(o => o.EndLine);
            }
            string linetxt;
            while ((linetxt = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(linetxt)) continue;
                if (num >= beginLine && txtList.Count < pageSize)
                    txtList.Add(linetxt);
                if (countLine > 0 && txtList.Count == pageSize) break;
                num++;
            }
            if (countLine == 0) countLine = num - 1;
            return txtList;
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetDataList_综合解释成果曲线数据(string PROCESS_ID)
        {
            return DbHelperOra.Query("select * from COM_LOG_COM_CURVEDATA where PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter("PROCESS_ID", OracleDbType.Varchar2, PROCESS_ID));
        }

        [WebMethod(EnableSession = true)]
        public int SaveDataList_测井分层数据(string PROCESS_ID, DataTable dt)
        {
            Workflow.Controller.ValidateSave<Workflow.C解释处理作业>(PROCESS_ID);

            StringBuilder strSql = new StringBuilder();
            object maxid = DbHelperOra.GetSingle("select max(LAYERID) from COM_LOG_LAYER where PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter("PROCESS_ID", OracleDbType.Varchar2, PROCESS_ID));
            strSql.Append("insert into COM_LOG_LAYER(");
            strSql.Append("LAYERID,PROCESS_ID,FORMATION_NAME,ST_DEVIATED_DEP,EN_DEVIATED_DEP");
            strSql.Append(") values (");
            strSql.Append("LAYERID_SEQ.nextval,:PROCESS_ID,:FORMATION_NAME,:ST_DEVIATED_DEP,:EN_DEVIATED_DEP");
            strSql.Append(") ");

            int i = DbHelperOra.InsertDataTable(strSql.ToString(), dt,
                        ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, PROCESS_ID),
                        ServiceUtils.CreateOracleParameter(":FORMATION_NAME", "FORMATION_NAME", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":ST_DEVIATED_DEP", "ST_DEVIATED_DEP", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":EN_DEVIATED_DEP", "EN_DEVIATED_DEP", OracleDbType.Decimal)
                        );
            DbHelperOra.ExecuteSql("delete from COM_LOG_LAYER where PROCESS_ID=:PROCESS_ID and LAYERID<=:LAYERID",
                ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, PROCESS_ID),
                ServiceUtils.CreateOracleParameter(":LAYERID", OracleDbType.Decimal, maxid)
                );
            return i;
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetDataList_测井分层数据(string PROCESS_ID)
        {
            return DbHelperOra.Query("select * from COM_LOG_LAYER where PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter("PROCESS_ID", OracleDbType.Varchar2, PROCESS_ID));
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetAllProcessFileByWellName(string well_name)
        {
            var ds = new DataSet();
            var dt = DbHelperOra.Query("select distinct (e.sha1||'-'||e.md5||'-'||e.length) hashname,e.length,e.pathid,e.pathmain,a.process_name,d.fileid,d.filename,i.filepath,i.pathid pathid_1 from dm_log_process a,com_well_basic b,sys_processing_uploadfile c,sys_file_upload d,sys_upload e,com_job_info f,sys_file_path i where a.drill_job_id=f.drill_job_id and f.well_id=b.well_id and c.process_id=a.process_id and d.fileid=c.fileid and e.uploadid=d.uploadid and i.pathid=d.pathid and b.well_name=:well_name",
             ServiceUtils.CreateOracleParameter(":well_name", OracleDbType.Varchar2, well_name)).Tables[0];
            dt.Columns.Add("filestate", typeof(decimal));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var path = FileUpload.PathList.FirstOrDefault(o => o.id == dt.Rows[i].FieldEx<decimal>("pathid")).name;
                var hashname = dt.Rows[i].FieldEx<string>("hashname");
                path = path.Remove(path.LastIndexOf('\\') + 1);
                path += dt.Rows[0].FieldEx<string>("pathmain");
                var filepath = string.Format("{0}\\{1}", path, hashname);
                if (File.Exists(filepath))
                    dt.Rows[i]["filestate"] = 1;
                else
                    dt.Rows[i]["filestate"] = 0;
            }
            ds.Tables.Add(dt.Copy());
            return ds;
        }

        [WebMethod(EnableSession = true)]
        public void UpdateProcessFileInfo(byte[] data)
        {
            bool isExists;
            var fileIds = ModelHelper.DeserializeObject(data) as Dictionary<decimal, decimal>;
            foreach (var o_id in fileIds.Keys)
            {
                if (o_id == fileIds[o_id]) continue;
                var op = ServiceUtils.CreateOracleParameter(":old_id", OracleDbType.Decimal, o_id);
                //更新关联文件id
                DbHelperOra.ExecuteSql("update sys_processing_uploadfile set fileid=:new_id where fileid=:old_id", op,
                    ServiceUtils.CreateOracleParameter(":new_id", OracleDbType.Decimal, fileIds[o_id]));

                var dt = DbHelperOra.Query("select uploadid,pathid from sys_file_upload where fileid=:old_id", op).Tables[0];
                var uploadid = dt.Rows[0].FieldEx<decimal>("uploadid");
                var pathid = dt.Rows[0].FieldEx<decimal>("pathid");
                var op1 = ServiceUtils.CreateOracleParameter(":uploadid", OracleDbType.Decimal, uploadid);
                var op2 = ServiceUtils.CreateOracleParameter(":pathid", OracleDbType.Decimal, pathid);

                //删除旧数据
                DbHelperOra.ExecuteSql("delete from sys_file_upload where fileid=:old_id", op);
                isExists = DbHelperOra.Exists("select count(1) from sys_file_upload where rownum<2 and uploadid=:uploadid", op1);
                if (!isExists)
                    DbHelperOra.ExecuteSql("delete from sys_upload where uploadid=:uploadid", op1);
                isExists = DbHelperOra.Exists("select count(1) from sys_file_upload where rownum<2 and pathid=:pathid", op2);
                if (!isExists)
                    DbHelperOra.ExecuteSql("delete from sys_file_path where pathid=:pathid", op2);
            }
        }

        [WebMethod(EnableSession = true)]
        public void DelSysUpload(DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var fileid = dt.Rows[i].FieldEx<decimal>("File_Id");
                var uploadid = dt.Rows[i].FieldEx<decimal>("Upload_Id");
                DbHelperOra.ExecuteSql("delete from sys_file_upload where fileid=:fileid", ServiceUtils.CreateOracleParameter(":fileid", OracleDbType.Decimal, fileid));
                var op = ServiceUtils.CreateOracleParameter(":uploadid", OracleDbType.Decimal, uploadid);
                bool isExists = DbHelperOra.Exists("select count(1) from sys_file_upload where rownum<2 and uploadid=:uploadid", op);
                if (!isExists)
                    DbHelperOra.ExecuteSql("delete from sys_upload where uploadid=:uploadid", op);
            }
        }

        [WebMethod(EnableSession = true)]
        public void DelUploadFile(DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var filePath = dt.Rows[i].FieldEx<string>("File_Path");
                File.Delete(filePath);
            }
        }

        [WebMethod(EnableSession = true)]
        public DataSet CompareDataFiles(decimal cType)
        {
            //获取数据库所有文件信息
            var ds = new DataSet();
            var dt = DbHelperOra.Query("select distinct (a.sha1||'-'||a.md5||'-'||a.length) hashname,b.fileid,b.uploadid,b.filename from sys_upload a,sys_file_upload b where a.uploadid=b.uploadid").Tables[0];
            var path = FileUpload.PathList[0].name;
            path = path.Remove(path.LastIndexOf('\\') + 1);
            var basedir = new DirectoryInfo(path);
            //获取上传目录所有文件
            var list = new List<FileInfo>();
            foreach (var dir in basedir.GetDirectories())
            {
                var files = dir.GetFiles();
                if (files.Length > 0)
                    list.AddRange(files);
            }
            //构造新的DataTable
            var result_dt = new DataTable();
            result_dt.Columns.Add("Well_Name", typeof(string));
            result_dt.Columns.Add("Process_Name", typeof(string));
            result_dt.Columns.Add("File_Name", typeof(string));
            result_dt.Columns.Add("File_Path", typeof(string));
            result_dt.Columns.Add("Upload_Id", typeof(decimal));
            result_dt.Columns.Add("File_Id", typeof(decimal));

            if (cType == 1)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var hashname = dt.Rows[i].FieldEx<string>("hashname");
                    var fileid = dt.Rows[i].FieldEx<decimal>("fileid");
                    var uploadid = dt.Rows[i].FieldEx<decimal>("uploadid");
                    var filename = dt.Rows[i].FieldEx<string>("filename");
                    var isExists = list.Exists(x => x.Name.Equals(hashname));
                    if (isExists) continue;
                    var op = ServiceUtils.CreateOracleParameter(":fileid", OracleDbType.Decimal, fileid);
                    var dt1 = DbHelperOra.Query("select c.process_name,e.well_name from sys_file_upload a,sys_processing_uploadfile b,dm_log_process c,com_job_info d,com_well_basic e where b.fileid=a.fileid and c.process_id=b.process_id and d.drill_job_id=c.drill_job_id and e.well_id=d.well_id and a.fileid=:fileid", op).Tables[0];
                    if (dt1.Rows.Count > 0)
                    {
                        var dr = result_dt.NewRow();
                        dr["File_Id"] = fileid;
                        dr["Upload_Id"] = uploadid;
                        dr["Well_Name"] = dt1.Rows[0].FieldEx<string>("well_name");
                        dr["Process_Name"] = dt1.Rows[0].FieldEx<string>("process_name");
                        dr["File_Name"] = filename;
                        result_dt.Rows.Add(dr);
                        continue;
                    }

                    //判断是否井信息表中文件
                    var well_name = DbHelperOra.GetSingle("select well_name from com_well_basic where drill_eng_des_fileid=:fileid or drill_geo_des_fileid=:fileid", op);
                    if (well_name != null)
                    {
                        var dr = result_dt.NewRow();
                        dr["File_Id"] = fileid;
                        dr["Upload_Id"] = uploadid;
                        dr["Well_Name"] = well_name;
                        dr["File_Name"] = filename;
                        result_dt.Rows.Add(dr);
                        continue;
                    }

                    //判断是否任务通知书表中文件
                    well_name = DbHelperOra.GetSingle("select b.well_name from dm_log_task a,com_well_basic b,com_job_info c where c.drill_job_id=a.drill_job_id and b.well_id=c.well_id and a.requisition_scanning_fileid=:fileid", op);
                    if (well_name != null)
                    {
                        var dr = result_dt.NewRow();
                        dr["File_Id"] = fileid;
                        dr["Upload_Id"] = uploadid;
                        dr["Well_Name"] = well_name;
                        dr["File_Name"] = filename;
                        result_dt.Rows.Add(dr);
                        continue;
                    }

                    //判断是否计划任务书表中文件
                    well_name = DbHelperOra.GetSingle("select d.well_name from dm_log_ops_plan a,dm_log_task b,com_job_info c,com_well_basic d where b.requisition_cd=a.requisition_cd and c.drill_job_id=b.drill_job_id and d.well_id=b.well_id and a.plan_content_scanning_fileid=:fileid", op);
                    if (well_name != null)
                    {
                        var dr = result_dt.NewRow();
                        dr["File_Id"] = fileid;
                        dr["Upload_Id"] = uploadid;
                        dr["Well_Name"] = well_name;
                        dr["File_Name"] = filename;
                        result_dt.Rows.Add(dr);
                        continue;
                    }

                    well_name = DbHelperOra.GetSingle("select d.well_name from pro_log_rapid_results a,dm_log_task b,com_job_info c,com_well_basic d where b.requisition_cd=a.requisition_cd and c.drill_job_id=b.drill_job_id and d.well_id=b.well_id and a.fileid=:fileid", op);
                    if (well_name != null)
                    {
                        var dr = result_dt.NewRow();
                        dr["File_Id"] = fileid;
                        dr["Upload_Id"] = uploadid;
                        dr["Well_Name"] = well_name;
                        dr["File_Name"] = filename;
                        result_dt.Rows.Add(dr);
                        continue;
                    }

                    well_name = DbHelperOra.GetSingle("select e.well_name from pro_log_rapid_original_data a,dm_log_ops_plan b,dm_log_task c,com_job_info d,com_well_basic e where b.job_plan_cd=a.job_plan_cd and c.requisition_cd=b.requisition_cd and d.drill_job_id=c.drill_job_id and e.well_id=d.well_id and a.fileid=:fileid", op);
                    if (well_name != null)
                    {
                        var dr = result_dt.NewRow();
                        dr["File_Id"] = fileid;
                        dr["Upload_Id"] = uploadid;
                        dr["Well_Name"] = well_name;
                        dr["File_Name"] = filename;
                        result_dt.Rows.Add(dr);
                        continue;
                    }
                    var dr_1 = result_dt.NewRow();
                    dr_1["File_Id"] = fileid;
                    dr_1["Upload_Id"] = uploadid;
                    dr_1["Well_Name"] = "不存在";
                    dr_1["File_Name"] = filename;
                    result_dt.Rows.Add(dr_1);
                }
            }
            else
            {
                foreach (var li in list)
                {
                    var d_row = dt.AsEnumerable().Where(x => x.FieldEx<string>("hashname").Equals(li.Name)).FirstOrDefault();
                    if (d_row != null) continue;
                    var dr = result_dt.NewRow();
                    dr["File_Name"] = li.Name;
                    dr["File_Path"] = li.FullName;
                    result_dt.Rows.Add(dr);
                }
            }
            ds.Tables.Add(result_dt.Copy());
            return ds;
        }

        [WebMethod(EnableSession = true)]
        public int SaveDataList_井眼轨迹数据(string PROCESS_ID, DataTable dt)
        {
            Workflow.Controller.ValidateSave<Workflow.C解释处理作业>(PROCESS_ID);

            StringBuilder strSql = new StringBuilder();
            object maxid = DbHelperOra.GetSingle("select max(TRAJECTORYID) from COM_LOG_WELL_TRAJECTORY where PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter("PROCESS_ID", OracleDbType.Varchar2, PROCESS_ID));
            strSql.Append("insert into COM_LOG_WELL_TRAJECTORY(");
            strSql.Append("TRAJECTORYID,PROCESS_ID,MD,INCLNATION,AZIMUTH,E_ELEMENT,N_ELEMENT,TVD,ALL_MOV,P_XN_CLOSURE_AZIMUTH,P_XN_CLOSED_DISTANCE,DOG_LEG");
            strSql.Append(") values (");
            strSql.Append("TRAJECTORYID_SEQ.nextval,:PROCESS_ID,:MD,:INCLNATION,:AZIMUTH,:E_ELEMENT,:N_ELEMENT,:TVD,:ALL_MOV,:P_XN_CLOSURE_AZIMUTH,:P_XN_CLOSED_DISTANCE,:DOG_LEG");
            strSql.Append(") ");
            int i = DbHelperOra.InsertDataTable(strSql.ToString(), dt,
                        ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, PROCESS_ID),
                        ServiceUtils.CreateOracleParameter(":MD", "MD", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":INCLNATION", "INCLNATION", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":AZIMUTH", "AZIMUTH", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":E_ELEMENT", "E_ELEMENT", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":N_ELEMENT", "N_ELEMENT", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":TVD", "TVD", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":ALL_MOV", "ALL_MOV", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":P_XN_CLOSURE_AZIMUTH", "P_XN_CLOSURE_AZIMUTH", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":P_XN_CLOSED_DISTANCE", "P_XN_CLOSED_DISTANCE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":DOG_LEG", "DOG_LEG", OracleDbType.Decimal)
                        );
            DbHelperOra.ExecuteSql("delete from COM_LOG_WELL_TRAJECTORY where PROCESS_ID=:PROCESS_ID and TRAJECTORYID<=:TRAJECTORYID",
                ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, PROCESS_ID),
                ServiceUtils.CreateOracleParameter(":TRAJECTORYID", OracleDbType.Decimal, maxid)
                );
            return i;
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetDataList_井眼轨迹数据(string PROCESS_ID)
        {
            return DbHelperOra.Query("select * from COM_LOG_WELL_TRAJECTORY where PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter("PROCESS_ID", OracleDbType.Varchar2, PROCESS_ID));
        }

        [WebMethod(EnableSession = true)]
        public int SaveProcessingCurveInfo(string PROCESS_ID, string MAPS_CHINESE_NAME, DataTable dt)
        {
            Workflow.Controller.ValidateSave<Workflow.C解释处理作业>(PROCESS_ID);
            //object maxid = DbHelperOra.GetSingle("select max(CURVEID) from PRO_LOG_PROCESSING_CURVE_INDEX where PROCESS_ID=:PROCESS_ID and MAPS_CODING=:MAPS_CODING",
            //    ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, PROCESS_ID),
            //    ServiceUtils.CreateOracleParameter(":MAPS_CODING", OracleDbType.Varchar2, MAPS_CHINESE_NAME)
            //    );
            DbHelperOra.ExecuteSql("delete from PRO_LOG_PROCESSING_CURVE_INDEX where PROCESS_ID=:PROCESS_ID AND MAPS_CODING=:MAPS_CODING",
                ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, PROCESS_ID),
                ServiceUtils.CreateOracleParameter(":MAPS_CODING", OracleDbType.Varchar2, MAPS_CHINESE_NAME)
                );
            StringBuilder strSql = new StringBuilder();
            strSql.Append("declare V_CURVEID NUMBER(38,0);");
            strSql.Append("begin ");
            strSql.Append("select CURVEID_SEQ_2.nextval into V_CURVEID from dual;");
            strSql.Append("insert into PRO_LOG_PROCESSING_CURVE_INDEX(");
            strSql.Append("CURVEID,PROCESS_ID,PROCESSING_ITEM_ID,CURVE_CD, CURVE_NAME, DATA_INFO, CURVE_UNIT, CURVE_START_DEP, CURVE_END_DEP, CURVE_RLEV, CURVE_T_UNIT, CURVE_DATA_TYPE, CURVE_DATA_LENGTH, CURVE_T_SAMPLE, CURVE_T_MIN_VALUE, CURVE_T_MAX_VALUE, CURVE_T_RELV, DATA_SIZE,DATA_STORAGE_WAY,MAPS_CODING");
            strSql.Append(") values (");
            strSql.Append("V_CURVEID,:PROCESS_ID,:PROCESSING_ITEM_ID,:CURVE_CD, :CURVE_NAME, :DATA_INFO, :CURVE_UNIT, :CURVE_START_DEP, :CURVE_END_DEP, :CURVE_RLEV, :CURVE_T_UNIT, :CURVE_DATA_TYPE, :CURVE_DATA_LENGTH, :CURVE_T_SAMPLE, :CURVE_T_MIN_VALUE, :CURVE_T_MAX_VALUE, :CURVE_T_RELV, :DATA_SIZE,:DATA_STORAGE_WAY,:MAPS_CODING");
            strSql.Append("); ");
            strSql.Append("insert into PRO_LOG_PROCESSING_CURVE_DATA(CURVEID,BLOCK_NUMBER,BLOCK_SIZE,CURVE_DATA) values (V_CURVEID,:BLOCK_NUMBER,:BLOCK_SIZE,:CURVE_DATA);");
            strSql.Append("end;");
            bool isExists;
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["FILEID"] == DBNull.Value) continue;
                var fileDT = DbHelperOra.Query("select B.UPLOADID, A.PATHID from sys_file_upload A,sys_upload B where A.uploadid=B.uploadid and A.fileid=:fileid", ServiceUtils.CreateOracleParameter(":fileid", OracleDbType.Decimal, dr["FILEID"])).Tables[0];
                var uploadid = fileDT.Rows[0].FieldEx<decimal>("UPLOADID");
                var pathid = fileDT.Rows[0].FieldEx<decimal>("PATHID");
                DbHelperOra.ExecuteSql("delete from sys_upload where uploadid=:uploadid", ServiceUtils.CreateOracleParameter(":uploadid", OracleDbType.Decimal, uploadid));
                isExists = DbHelperOra.Exists("select count(1) from sys_file_upload where rownum<2 and pathid=:pathid", ServiceUtils.CreateOracleParameter(":pathid", OracleDbType.Decimal, pathid));
                if (!isExists)
                {
                    DbHelperOra.ExecuteSql("delete from sys_file_path where pathid=:pathid", ServiceUtils.CreateOracleParameter(":pathid", OracleDbType.Decimal, pathid));
                }
            }
            int i = DbHelperOra.InsertDataTable(strSql.ToString(), dt,
                         ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, PROCESS_ID),
                         ServiceUtils.CreateOracleParameter(":PROCESSING_ITEM_ID", "PROCESSING_ITEM_ID", OracleDbType.Decimal),
                         ServiceUtils.CreateOracleParameter(":CURVE_CD", "CURVE_CD", OracleDbType.Varchar2),
                         ServiceUtils.CreateOracleParameter(":CURVE_NAME", "CURVE_NAME", OracleDbType.Varchar2),
                         ServiceUtils.CreateOracleParameter(":CURVE_UNIT", "CURVE_UNIT", OracleDbType.Varchar2),
                         ServiceUtils.CreateOracleParameter(":DATA_INFO", "DATA_INFO", OracleDbType.Decimal),
                         ServiceUtils.CreateOracleParameter(":CURVE_START_DEP", "CURVE_START_DEP", OracleDbType.Decimal),
                         ServiceUtils.CreateOracleParameter(":CURVE_END_DEP", "CURVE_END_DEP", OracleDbType.Decimal),
                         ServiceUtils.CreateOracleParameter(":CURVE_RLEV", "CURVE_RLEV", OracleDbType.Decimal),
                         ServiceUtils.CreateOracleParameter(":CURVE_T_UNIT", "CURVE_T_UNIT", OracleDbType.Varchar2),
                         ServiceUtils.CreateOracleParameter(":CURVE_DATA_TYPE", "CURVE_DATA_TYPE", OracleDbType.Varchar2),
                         ServiceUtils.CreateOracleParameter(":CURVE_DATA_LENGTH", "CURVE_DATA_LENGTH", OracleDbType.Decimal),
                         ServiceUtils.CreateOracleParameter(":CURVE_T_SAMPLE", "CURVE_T_SAMPLE", OracleDbType.Decimal),
                         ServiceUtils.CreateOracleParameter(":CURVE_T_MIN_VALUE", "CURVE_T_MIN_VALUE", OracleDbType.Decimal),
                         ServiceUtils.CreateOracleParameter(":CURVE_T_MAX_VALUE", "CURVE_T_MAX_VALUE", OracleDbType.Decimal),
                         ServiceUtils.CreateOracleParameter(":CURVE_T_RELV", "CURVE_T_RELV", OracleDbType.Decimal),
                         ServiceUtils.CreateOracleParameter(":DATA_SIZE", "DATA_SIZE", OracleDbType.Decimal),
                         ServiceUtils.CreateOracleParameter(":DATA_STORAGE_WAY", "DATA_STORAGE_WAY", OracleDbType.Decimal),
                         ServiceUtils.CreateOracleParameter(":MAPS_CODING", OracleDbType.Varchar2, MAPS_CHINESE_NAME),
                         ServiceUtils.CreateOracleParameter(":CURVE_CD", "CURVE_CD", OracleDbType.Varchar2),
                         ServiceUtils.CreateOracleParameter(":BLOCK_NUMBER", "BLOCK_NUMBER", OracleDbType.Decimal),
                         ServiceUtils.CreateOracleParameter(":BLOCK_SIZE", "BLOCK_SIZE", OracleDbType.Decimal),
                         ServiceUtils.CreateOracleParameter(":CURVE_DATA", "CURVE_DATA", OracleDbType.Blob)
                         );
            //DbHelperOra.ExecuteSql("delete from PRO_LOG_PROCESSING_CURVE_INDEX where PROCESS_ID=:PROCESS_ID AND CURVEID<=:CURVEID",
            //    ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, PROCESS_ID),
            //    ServiceUtils.CreateOracleParameter(":CURVEID", OracleDbType.Decimal, maxid)
            //    );
            return i;
        }

        [WebMethod(EnableSession = true)]
        public int SaveCurveInfo(string PROCESS_ID, decimal PROCESSING_ITEM_ID, DataTable dt)
        {
            Workflow.Controller.ValidateSave<Workflow.C解释处理作业>(PROCESS_ID);

            StringBuilder strSql = new StringBuilder();
            object maxid = DbHelperOra.GetSingle("select max(CURVEID) from PRO_LOG_PROCESSING_CURVE_INDEX where PROCESS_ID=:PROCESS_ID",
                ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, PROCESS_ID)
                );
            strSql.Append("declare V_CURVEID NUMBER(38,0);");
            strSql.Append("begin ");
            strSql.Append("select CURVEID_SEQ_2.nextval into V_CURVEID from dual;");
            strSql.Append("insert into PRO_LOG_PROCESSING_CURVE_INDEX(");
            strSql.Append("CURVEID,PROCESS_ID,PROCESSING_ITEM_ID,CURVE_CD, CURVE_NAME, DATA_INFO, CURVE_UNIT, CURVE_START_DEP, CURVE_END_DEP, CURVE_RLEV, CURVE_T_UNIT, CURVE_DATA_TYPE, CURVE_DATA_LENGTH, CURVE_T_SAMPLE, CURVE_T_MIN_VALUE, CURVE_T_MAX_VALUE, CURVE_T_RELV, DATA_SIZE");
            strSql.Append(") values (");
            strSql.Append("V_CURVEID,:PROCESS_ID,:PROCESSING_ITEM_ID,:CURVE_CD, :CURVE_NAME, :DATA_INFO, :CURVE_UNIT, :CURVE_START_DEP, :CURVE_END_DEP, :CURVE_RLEV, :CURVE_T_UNIT, :CURVE_DATA_TYPE, :CURVE_DATA_LENGTH, :CURVE_T_SAMPLE, :CURVE_T_MIN_VALUE, :CURVE_T_MAX_VALUE, :CURVE_T_RELV, :DATA_SIZE");
            strSql.Append("); ");
            strSql.Append("insert into PRO_LOG_PROCESSING_CURVE_DATA(CURVEID,BLOCK_NUMBER,BLOCK_SIZE,CURVE_DATA) values (V_CURVEID,:BLOCK_NUMBER,:BLOCK_SIZE,:CURVE_DATA);");
            strSql.Append("end;");
            int i = DbHelperOra.InsertDataTable(strSql.ToString(), dt,
                        ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, PROCESS_ID),
                        ServiceUtils.CreateOracleParameter(":PROCESSING_ITEM_ID", OracleDbType.Decimal, PROCESSING_ITEM_ID),
                        ServiceUtils.CreateOracleParameter(":CURVE_CD", "CURVE_CD", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":CURVE_NAME", "CURVE_NAME", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":CURVE_UNIT", "CURVE_UNIT", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":DATA_INFO", "DATA_INFO", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURVE_START_DEP", "CURVE_START_DEP", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURVE_END_DEP", "CURVE_END_DEP", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURVE_RLEV", "CURVE_RLEV", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURVE_T_UNIT", "CURVE_T_UNIT", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":CURVE_DATA_TYPE", "CURVE_DATA_TYPE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":CURVE_DATA_LENGTH", "CURVE_DATA_LENGTH", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURVE_T_SAMPLE", "CURVE_T_SAMPLE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURVE_T_MIN_VALUE", "CURVE_T_MIN_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURVE_T_MAX_VALUE", "CURVE_T_MAX_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURVE_T_RELV", "CURVE_T_RELV", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":DATA_SIZE", "DATA_SIZE", OracleDbType.Decimal),
                //ServiceUtils.CreateOracleParameter(":FILEID", "FILEID", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURVE_CD", "CURVE_CD", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":BLOCK_NUMBER", "BLOCK_NUMBER", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":BLOCK_SIZE", "BLOCK_SIZE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURVE_DATA", "CURVE_DATA", OracleDbType.Blob)
                        );

            DbHelperOra.ExecuteSql("delete from PRO_LOG_PROCESSING_CURVE_INDEX where PROCESS_ID=:PROCESS_ID AND CURVEID<=:CURVEID",
                ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, PROCESS_ID),
                ServiceUtils.CreateOracleParameter(":CURVEID", OracleDbType.Decimal, maxid)
                );
            return i;
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetCurveInfo(string PROCESS_ID)
        {
            return DbHelperOra.Query("SELECT * FROM PRO_LOG_PROCESSING_CURVE_INDEX where PROCESS_ID=:PROCESS_ID",
                ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, PROCESS_ID)
                );
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetProcessingCurveData(decimal CURVE_ID)
        {
            return DbHelperOra.Query("SELECT * FROM PRO_LOG_PROCESSING_CURVE_DATA where CURVEID=:CURVEID",
                ServiceUtils.CreateOracleParameter(":CURVEID", OracleDbType.Decimal, CURVE_ID)
                );
        }

        [WebMethod(EnableSession = true)]
        public bool DelProcessingFile(string PROCESS_ID, int filetype, DataTable newdt)
        {
            var ds1 = DbHelperOra.Query("select distinct a.UPLOADID,a.PATHID,a.FILENAME from SYS_FILE_UPLOAD a,SYS_PROCESSING_UPLOADFILE b where a.fileid=b.fileid and b.PROCESS_ID=:PROCESS_ID AND b.FILETYPE=:FILETYPE",
                ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, PROCESS_ID),
                ServiceUtils.CreateOracleParameter(":FILETYPE", OracleDbType.Decimal, filetype)
                );
            DbHelperOra.Query("delete from SYS_PROCESSING_UPLOADFILE where PROCESS_ID=" + PROCESS_ID + " AND FILETYPE=" + filetype);

            if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                StringBuilder file_ids = new StringBuilder();
                StringBuilder UPLOADID_ids = new StringBuilder();
                StringBuilder pathid_ids = new StringBuilder();
                bool delpathid = true;
                if (newdt.Rows.Count > 0)
                {
                    for (int i = 0; i < newdt.Rows.Count; i++)
                    {
                        var fid = newdt.Rows[i]["FILEID"].ToString();
                        if (!string.IsNullOrEmpty(fid))
                        {
                            file_ids.Append(fid);
                            file_ids.Append(",");
                        }
                    }
                }
                if (file_ids.Length > 1) file_ids = file_ids.Remove(file_ids.Length - 1, 1);
                var ds3 = DbHelperOra.Query("select distinct UPLOADID from SYS_FILE_UPLOAD where fileid in(" + file_ids + ")").Tables[0];
                if (ds3.Rows.Count > 0)
                {
                    for (int i = 0; i < ds3.Rows.Count; i++)
                    {
                        var UPL = ds3.Rows[i]["UPLOADID"].ToString();
                        if (!string.IsNullOrEmpty(UPL))
                        {
                            UPLOADID_ids.Append(UPL);
                            UPLOADID_ids.Append(",");
                        }
                    }
                }
                for (int j = 1; j < 6; j++)
                {
                    var dt2 = DbHelperOra.Query("select distinct bb.pathid from sys_processing_uploadfile aa,sys_file_upload bb where bb.fileid = aa.fileid and aa.FILETYPE=" + j + " and aa.process_id=" + PROCESS_ID).Tables[0];
                    if (dt2 != null && dt2.Rows.Count == 1)
                    {
                        var dt3 = DbHelperOra.Query("select * from sys_file_upload where pathid=" + dt2.Rows[0].FieldEx<decimal>("pathid")).Tables[0];
                        if (dt3 != null && dt3.Rows.Count == 1)
                        {
                            pathid_ids.Append(dt2.Rows[0].FieldEx<decimal>("pathid"));
                            pathid_ids.Append(",");
                        }
                    }
                }
                for (int n = 0; n < ds1.Tables[0].Rows.Count; n++)
                {
                    if (UPLOADID_ids.ToString().Contains(ds1.Tables[0].Rows[n]["UPLOADID"].ToString())) { delpathid = false; continue; }
                    int UPLOADID = Convert.ToInt32(ds1.Tables[0].Rows[n]["UPLOADID"].ToString());

                    var dt = DbHelperOra.Query("select sha1,md5,length,pathid,PATHMAIN from sys_upload where uploadid =" + UPLOADID).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        string sha1 = null, md5 = null, path = null, server_filepath;
                        decimal length = 0;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            sha1 = dt.Rows[i].FieldEx<string>("SHA1");
                            md5 = dt.Rows[i].FieldEx<string>("MD5");
                            length = dt.Rows[i].FieldEx<decimal>("LENGTH");

                            path = FileUpload.PathList.FirstOrDefault(o => o.id == dt.Rows[i].FieldEx<decimal>("PATHID")).name;
                            path = path.Remove(path.LastIndexOf('\\') + 1);
                            path += dt.Rows[i].FieldEx<string>("PATHMAIN");

                            server_filepath = string.Format("{0}\\{1}-{2}-{3}", path, sha1, md5, length);
                            try
                            {
                                //删除服务器上文件
                                if (File.Exists(server_filepath))
                                {
                                    File.Delete(server_filepath);
                                    File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "LogFile\\RetransmissionLog.txt", DateTime.Now + "：：" + ds1.Tables[0].Rows[n]["FILENAME"].ToString() + ",路径:" + server_filepath + "\r\n");
                                }
                            }
                            catch (Exception ex)
                            {
                                File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "LogFile\\RetransmissionLog_Error.txt", DateTime.Now + "：：" + ds1.Tables[0].Rows[n]["FILENAME"].ToString() + ",路径:" + server_filepath + ",异常信息：" + ex.Message + "\r\n");
                                return false;
                            }
                        }
                    }
                    //删除文件信息
                    DbHelperOra.Query("delete from sys_upload where UPLOADID=" + UPLOADID);
                }
                try
                {
                    if (!delpathid) return true;
                    if (pathid_ids.Length > 1)
                    {
                        pathid_ids = pathid_ids.Remove(pathid_ids.Length - 1, 1);
                        DbHelperOra.Query("delete from sys_file_path where PATHID in(" + pathid_ids + ")");
                        File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "LogFile\\sysfilepathLog.txt", DateTime.Now + "：：成功删除!\r\n");
                    }
                }
                catch (Exception ex)
                {
                    File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "LogFile\\sysfilepathLog_Error.txt", DateTime.Now + "：：异常信息：" + ex.Message + "\r\n");
                }
            }
            return true;
        }

        [WebMethod(EnableSession = true)]
        public int SaveProcessingFile(string PROCESS_ID, int filetype, DataTable dt)
        {
            if (!DelProcessingFile(PROCESS_ID, filetype, dt)) return 0;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into SYS_PROCESSING_UPLOADFILE(");
            strSql.Append("PROCESS_UPLOAD_ID,PROCESS_ID,EXTENSION,FILETYPE,FILEID,PROCESSING_ITEM_ID,PROCESS_UPLOAD_FILE");
            strSql.Append(") values (");
            strSql.Append("PROCESS_UPLOAD_ID_SEQ.NEXTVAL,:PROCESS_ID,:EXTENSION,:FILETYPE,:FILEID,:PROCESSING_ITEM_ID,:PROCESS_UPLOAD_FILE");
            strSql.Append(") ");
            int i = DbHelperOra.InsertDataTable(strSql.ToString(), dt,
                        ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, PROCESS_ID),
                        ServiceUtils.CreateOracleParameter(":EXTENSION", "EXTENSION", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":FILETYPE", OracleDbType.Decimal, filetype),
                        ServiceUtils.CreateOracleParameter(":FILEID", "FILEID", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":PROCESSING_ITEM_ID", "PROCESSING_ITEM_ID", OracleDbType.Decimal),
                //保存文件到数据库
                        ServiceUtils.CreateOracleParameter(":PROCESS_UPLOAD_FILE", "PROCESS_UPLOAD_FILE", OracleDbType.Blob)
                        );
            return i;
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetProcessingFile(string PROCESS_ID, int filetype)
        {
            return DbHelperOra.Query("SELECT a.fileid,a.extension,a.PROCESSING_ITEM_ID,b.filename,c.length FROM SYS_PROCESSING_UPLOADFILE a,SYS_FILE_UPLOAD b,SYS_UPLOAD c where PROCESS_ID=:PROCESS_ID AND FILETYPE=:FILETYPE AND a.FILEID=b.FILEID AND b.UPLOADID=c.UPLOADID",
                ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, PROCESS_ID),
                ServiceUtils.CreateOracleParameter(":FILETYPE", OracleDbType.Decimal, filetype)
                );
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetProcessDocument(string PROCESS_ID, int filetype)
        {
            return DbHelperOra.Query("SELECT a.fileid,a.extension,a.PROCESSING_ITEM_ID,b.filename,c.length,d.DOCUMENT_TYPE,d.A1_GUID,d.DOCUMENT_WRITER,d.DOCUMENT_VERIFIER,d.DOCUMENT_COMPLETION_DATE FROM SYS_PROCESSING_UPLOADFILE a,SYS_FILE_UPLOAD b,SYS_UPLOAD c,pro_log_process_document d where a.PROCESS_ID=:PROCESS_ID AND FILETYPE=:FILETYPE AND a.FILEID=b.FILEID AND b.UPLOADID=c.UPLOADID AND a.FILEID=d.FILEID(+)",
                ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, PROCESS_ID),
                ServiceUtils.CreateOracleParameter(":FILETYPE", OracleDbType.Decimal, filetype)
                );
        }

        [WebMethod(EnableSession = true)]
        public int SaveProcessDocument(string PROCESS_ID, int filetype, DataTable dt)
        {
            if (!DelProcessingFile(PROCESS_ID, filetype, dt)) return 0;
            var param = ServiceUtils.CreateOracleParameter(":process_id", OracleDbType.Varchar2, PROCESS_ID);
            var oldDt = DbHelperOra.Query("select a1_guid,fileid from pro_log_process_document where process_id=:process_id", param).Tables[0];
            if (oldDt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var dr = dt.Rows[i];
                    var row = oldDt.AsEnumerable().Where(x => x.FieldEx<string>("FILEID").Equals(dr["FILEID"])).FirstOrDefault();
                    if (row == null || row["A1_GUID"] == null)
                        dr["A1_GUID"] = Guid.NewGuid().ToString();
                    else
                        dr["A1_GUID"] = row["A1_GUID"].ToString();
                }
                DbHelperOra.ExecuteSql("delete from pro_log_process_document where process_id=:process_id", param);
            }
            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var dr = dt.Rows[i];
                    dr["A1_GUID"] = Guid.NewGuid().ToString();
                }
            }

            StringBuilder strSql = new StringBuilder();
            strSql.Append("begin ");
            strSql.Append("insert into SYS_PROCESSING_UPLOADFILE(");
            strSql.Append("PROCESS_UPLOAD_ID,PROCESS_ID,EXTENSION,FILETYPE,FILEID,PROCESSING_ITEM_ID,PROCESS_UPLOAD_FILE");
            strSql.Append(") values (");
            strSql.Append("PROCESS_UPLOAD_ID_SEQ.NEXTVAL,:PROCESS_ID,:EXTENSION,:FILETYPE,:FILEID,:PROCESSING_ITEM_ID,:PROCESS_UPLOAD_FILE");
            strSql.Append("); ");
            strSql.Append("insert into PRO_LOG_PROCESS_DOCUMENT(");
            strSql.Append("A1_GUID,FILEID,PROCESS_ID,DOCUMENT_NAME,DOCUMENT_TYPE,DOCUMENT_FORMAT,DOCUMENT_WRITER,DOCUMENT_VERIFIER,DOCUMENT_COMPLETION_DATE,DOCUMENT_DATA_SIZE");
            strSql.Append(") values (");
            strSql.Append(":A1_GUID,:FILEID,:PROCESS_ID,:DOCUMENT_NAME,:DOCUMENT_TYPE,:DOCUMENT_FORMAT,:DOCUMENT_WRITER,:DOCUMENT_VERIFIER,:DOCUMENT_COMPLETION_DATE,:DOCUMENT_DATA_SIZE");
            strSql.Append("); ");
            strSql.Append("end;");
            int j = DbHelperOra.InsertDataTable(strSql.ToString(), dt,
                        ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, PROCESS_ID),
                        ServiceUtils.CreateOracleParameter(":EXTENSION", "DOCUMENT_FORMAT", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":FILETYPE", OracleDbType.Decimal, filetype),
                        ServiceUtils.CreateOracleParameter(":FILEID", "FILEID", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":PROCESSING_ITEM_ID", "PROCESSING_ITEM_ID", OracleDbType.Decimal),
                //保存文件到数据库
                        ServiceUtils.CreateOracleParameter(":PROCESS_UPLOAD_FILE", "PROCESS_UPLOAD_FILE", OracleDbType.Blob),
                        ServiceUtils.CreateOracleParameter(":A1_GUID", "A1_GUID", OracleDbType.NVarchar2),
                        ServiceUtils.CreateOracleParameter(":DOCUMENT_NAME", "DOCUMENT_NAME", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":DOCUMENT_TYPE", "DOCUMENT_TYPE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":DOCUMENT_FORMAT", "DOCUMENT_FORMAT", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":DOCUMENT_WRITER", "DOCUMENT_WRITER", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":DOCUMENT_VERIFIER", "DOCUMENT_VERIFIER", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":DOCUMENT_COMPLETION_DATE", "DOCUMENT_COMPLETION_DATE", OracleDbType.TimeStamp),
                        ServiceUtils.CreateOracleParameter(":DOCUMENT_DATA_SIZE", "DOCUMENT_DATA_SIZE", OracleDbType.Decimal));
            return j;
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetMyTask()
        {
            var ds = new DataSet();
            //var dt1 = DbHelperOra.Query("select distinct a.requisition_cd from dm_log_task a,sys_work_flow_now b,hs_role c where a.requisition_cd=b.obj_id and b.flow_type=0 and b.target_loginname= :LOGINNAME and b.target_loginname=c.col_loginname and c.col_roletype=0 AND b.source_loginname<>b.target_loginname", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":LOGINNAME", OracleDbType.Varchar2, ServiceUtils.GetUserInfo().COL_LOGINNAME) }).Tables[0];
            var dt1 = DbHelperOra.Query("select distinct a.requisition_cd from dm_log_task a,sys_work_flow_now b,hs_role c where a.requisition_cd=b.obj_id and b.flow_type=0 and b.target_loginname= :LOGINNAME and b.target_loginname=c.col_loginname and c.col_roletype=0", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":LOGINNAME", OracleDbType.Varchar2, ServiceUtils.GetUserInfo().COL_LOGINNAME) }).Tables[0];
            var dt2 = DbHelperOra.Query("select distinct a.job_plan_cd from dm_log_ops_plan a,(select obj_id,flow_id from sys_work_flow_now where flow_type =0) b,sys_work_flow_now c where a.requisition_cd=b.obj_id and c.flow_id=b.flow_id and a.job_plan_cd not in (select obj_id from sys_work_flow_now where flow_type=1) and c.target_loginname=:LOGINNAME and (c.source_loginname=c.target_loginname or c.target_loginname not in (select col_loginname from hs_role where col_roletype=0)) UNION ALL select distinct a.job_plan_cd from dm_log_ops_plan a,(select obj_id,flow_id from sys_work_flow_now where flow_type =1) b,sys_work_flow_now c where a.job_plan_cd=b.obj_id and c.flow_id=b.flow_id and ((c.target_loginname= :LOGINNAME and c.flow_state in (1,3,6)) or (c.source_loginname=:LOGINNAME and c.flow_state =5))", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":LOGINNAME", OracleDbType.Varchar2, ServiceUtils.GetUserInfo().COL_LOGINNAME) }).Tables[0];
            var dt3 = DbHelperOra.Query("SELECT DISTINCT a.job_plan_cd FROM dm_log_ops_plan a,(SELECT obj_id,flow_id FROM sys_work_flow_now WHERE flow_type =1) b,sys_work_flow_now c WHERE a.job_plan_cd =b.obj_id AND c.flow_id  =b.flow_id AND c.flow_state =2 AND a.job_plan_cd NOT IN(SELECT obj_id FROM sys_work_flow_now WHERE flow_type=4)AND c.target_loginname      =:LOGINNAME AND(c.source_loginname=c.target_loginname or c.target_loginname NOT IN(SELECT col_loginname FROM hs_role WHERE col_roletype=0))UNION ALL SELECT DISTINCT a.job_plan_cd FROM dm_log_ops_plan a,(SELECT obj_id,flow_id FROM sys_work_flow_now WHERE flow_type =4) b,sys_work_flow_now c WHERE a.job_plan_cd=b.obj_id AND c.flow_id=b.flow_id AND ((c.target_loginname= :LOGINNAME AND c.flow_state in (3,6)) or (c.source_loginname=:LOGINNAME and c.flow_state =5))", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":LOGINNAME", OracleDbType.Varchar2, ServiceUtils.GetUserInfo().COL_LOGINNAME) }).Tables[0];
            var dt4 = DbHelperOra.Query("SELECT DISTINCT a.job_plan_cd FROM dm_log_ops_plan a,(SELECT obj_id,flow_id FROM sys_work_flow_now WHERE flow_type =1) b,sys_work_flow_now c WHERE a.job_plan_cd =b.obj_id AND c.flow_id  =b.flow_id AND c.flow_state =2 AND a.job_plan_cd NOT IN(SELECT obj_id FROM sys_work_flow_now WHERE flow_type=4)AND c.target_loginname      =:LOGINNAME AND(c.source_loginname=c.target_loginname or c.target_loginname NOT IN(SELECT col_loginname FROM hs_role WHERE col_roletype=0))UNION ALL SELECT DISTINCT a.job_plan_cd FROM dm_log_ops_plan a,(SELECT obj_id,flow_id FROM sys_work_flow_now WHERE flow_type =4) b,sys_work_flow_now c WHERE a.job_plan_cd=b.obj_id AND c.flow_id=b.flow_id AND ((c.target_loginname= :LOGINNAME AND c.flow_state IN (1,3,6)) or (c.source_loginname=:LOGINNAME and c.flow_state in(2,5)))", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":LOGINNAME", OracleDbType.Varchar2, ServiceUtils.GetUserInfo().COL_LOGINNAME) }).Tables[0];
            var dt5 = DbHelperOra.Query("SELECT DISTINCT a.process_name FROM dm_log_process a,(SELECT obj_id,flow_id FROM sys_work_flow_now WHERE flow_type =5) b,sys_work_flow_now c WHERE a.process_id   =b.obj_id AND c.flow_id =b.flow_id AND ((c.target_loginname= :LOGINNAME AND c.flow_state in (3,6)) or (c.source_loginname=:LOGINNAME AND c.flow_state IN(4,5)))", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":LOGINNAME", OracleDbType.Varchar2, ServiceUtils.GetUserInfo().COL_LOGINNAME) }).Tables[0];
            var dt6 = DbHelperOra.Query("SELECT DISTINCT a.process_name FROM dm_log_process a,(SELECT obj_id,flow_id FROM sys_work_flow_now WHERE flow_type =5) b,sys_work_flow_now c WHERE a.process_id   =b.obj_id AND c.flow_id =b.flow_id AND ((c.target_loginname= :LOGINNAME AND c.flow_state IN (1,3,6)) or (c.source_loginname=:LOGINNAME AND c.flow_state IN(4,5)))", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":LOGINNAME", OracleDbType.Varchar2, ServiceUtils.GetUserInfo().COL_LOGINNAME) }).Tables[0];
            var dt7 = DbHelperOra.Query("select distinct obj_id as job_plan_cd from sys_work_flow_now where flow_type=4 and flow_state=0 and target_loginname=:LOGINNAME and obj_id not in (select job_plan_cd from dm_log_source_data)", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":LOGINNAME", OracleDbType.Varchar2, ServiceUtils.GetUserInfo().COL_LOGINNAME) }).Tables[0];
            dt1.TableName = "DM_LOG_TASK";
            dt2.TableName = "DM_LOG_OPS_PLAN";
            dt3.TableName = "小队施工信息";
            dt4.TableName = "测井现场提交信息";
            dt5.TableName = "解释处理作业";
            dt6.TableName = "归档入库";
            dt7.TableName = "接收到的数据";
            ds.Tables.Add(dt1.Copy());
            ds.Tables.Add(dt2.Copy());
            ds.Tables.Add(dt3.Copy());
            ds.Tables.Add(dt4.Copy());
            ds.Tables.Add(dt5.Copy());
            ds.Tables.Add(dt6.Copy());
            ds.Tables.Add(dt7.Copy());
            return ds;
        }

        [WebMethod(EnableSession = true)]
        public bool DeleteLogTask(string REQUISITION_CD)
        {
            if (string.IsNullOrWhiteSpace(REQUISITION_CD)) return false;
            //if (!new UserService().GetActiveUserRoles().Contains(ServiceEnums.UserRole.系统管理员))
            //    ServiceUtils.ThrowSoapException("没有权限！");
            Workflow.Controller.ValidateDelete();
            List<int> fileids = new List<int>();
            var scan_fileid = DbHelperOra.GetSingle("select REQUISITION_SCANNING_FILEID from DM_LOG_TASK where REQUISITION_CD='" + REQUISITION_CD + "'");
            if (!string.IsNullOrWhiteSpace(Convert.ToString(scan_fileid))) fileids.Add(Convert.ToInt32(scan_fileid));
            var plan_dt = DbHelperOra.Query("select PLAN_CONTENT_SCANNING_FILEID from DM_LOG_OPS_PLAN  where REQUISITION_CD='" + REQUISITION_CD + "'").Tables[0];
            var result_dt = DbHelperOra.Query("select FILEID  from PRO_LOG_RAPID_RESULTS  where REQUISITION_CD='" + REQUISITION_CD + "'").Tables[0];
            var original_dt = DbHelperOra.Query("select FILEID  from PRO_LOG_RAPID_ORIGINAL_DATA where JOB_PLAN_CD like '" + REQUISITION_CD + "%'").Tables[0];

            for (int i = 0; i < plan_dt.Rows.Count; i++)
            {
                var fileid = Convert.ToString(plan_dt.Rows[i][0]);
                if (!string.IsNullOrWhiteSpace(fileid)) fileids.Add(Convert.ToInt32(fileid));
            }
            for (int i = 0; i < result_dt.Rows.Count; i++)
            {
                var fileid = Convert.ToString(result_dt.Rows[i][0]);
                if (!string.IsNullOrWhiteSpace(fileid)) fileids.Add(Convert.ToInt32(fileid));
            }
            for (int i = 0; i < original_dt.Rows.Count; i++)
            {
                var fileid = Convert.ToString(original_dt.Rows[i][0]);
                if (!string.IsNullOrWhiteSpace(fileid)) fileids.Add(Convert.ToInt32(fileid));
            }

            var process_id = DbHelperOra.GetSingle("select c.log_data_id from dm_log_task a,dm_log_ops_plan b,dm_log_source_data c where a.REQUISITION_CD=b.REQUISITION_CD and b.JOB_PLAN_CD=c.JOB_PLAN_CD and a.REQUISITION_CD=:REQUISITION_CD",
                      ServiceUtils.CreateOracleParameter(":REQUISITION_CD", OracleDbType.Varchar2, REQUISITION_CD));
            if (process_id != null)
            {
                var dt = DbHelperOra.Query("select fileid from sys_processing_uploadfile where process_id=:PROCESS_ID",
                    ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id)).Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var fileid = Convert.ToString(dt.Rows[i][0]);
                    if (!string.IsNullOrWhiteSpace(fileid)) fileids.Add(Convert.ToInt32(fileid));
                }
            }

            for (int i = 0; i < fileids.Count; i++)
            {
                var upload_dt = DbHelperOra.Query("select c.* from sys_processing_uploadfile a,sys_file_upload b,sys_upload c where a.fileid=b.fileid and b.uploadid=c.uploadid and a.fileid=:FILEID",
                    ServiceUtils.CreateOracleParameter(":FILEID", OracleDbType.Decimal, fileids[i])).Tables[0];
                if (upload_dt.Rows.Count > 0)
                {
                    var uploadid = upload_dt.Rows[0].FieldEx<decimal>("UPLOADID");
                    //删除sys_upload、sys_file_upload数据
                    DbHelperOra.ExecuteSql("delete from sys_file_upload where fileid=:FILEID",
                        ServiceUtils.CreateOracleParameter(":FILEID", OracleDbType.Decimal, fileids[i]));
                    var isExists = DbHelperOra.Exists("select count(1) from sys_file_upload where rownum<2 and uploadid=:UPLOADID",
                        ServiceUtils.CreateOracleParameter(":UPLOADID", OracleDbType.Decimal, uploadid));
                    if (isExists) continue;

                    var sha1 = upload_dt.Rows[0].FieldEx<string>("SHA1");
                    var md5 = upload_dt.Rows[0].FieldEx<string>("MD5");
                    var length = upload_dt.Rows[0].FieldEx<decimal>("LENGTH");
                    var path = FileUpload.PathList.FirstOrDefault(o => o.id == upload_dt.Rows[0].FieldEx<decimal>("PATHID")).name;
                    path = path.Remove(path.LastIndexOf('\\') + 1);
                    path += upload_dt.Rows[0].FieldEx<string>("PATHMAIN");
                    var server_filepath = string.Format("{0}\\{1}-{2}-{3}", path, sha1, md5, length);
                    DbHelperOra.Query("delete from sys_upload where uploadid=:UPLOADID",
                    ServiceUtils.CreateOracleParameter(":UPLOADID", OracleDbType.Decimal, uploadid));
                    try
                    {
                        //删除服务器上文件
                        if (File.Exists(server_filepath))
                        {
                            File.Delete(server_filepath);
                            File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "LogFile\\DeleteLogProcess.txt", DateTime.Now + "：：路径:" + server_filepath + "\r\n");
                        }
                    }
                    catch (Exception ex)
                    {
                        File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "LogFile\\DeleteLogProcess_Error.txt", DateTime.Now + "：：路径:" + server_filepath + ",异常信息：" + ex.Message + "\r\n");
                    }
                }
            }

            StringBuilder strSql = new StringBuilder();
            strSql.Append("begin ");
            //strSql.Append("delete from SYS_WORK_FLOW WHERE OBJ_ID=:REQUISITION_CD and flow_type=0;");
            strSql.Append("delete from SYS_WORK_FLOW WHERE OBJ_ID like :REQUISITION_CD || '%';");
            //strSql.Append("delete from SYS_WORK_FLOW WHERE OBJ_ID in (SELECT JOB_PLAN_CD FROM DM_LOG_OPS_PLAN WHERE requisition_cd=:REQUISITION_CD) and flow_type in(1,2,3,4);");
            strSql.Append("delete from dm_log_process where process_id=:PROCESS_ID;");
            strSql.Append("delete from DM_LOG_TASK WHERE REQUISITION_CD=:REQUISITION_CD;");
            strSql.Append("delete from sys_file_path where pathid not in(select pathid from sys_file_upload);");
            strSql.Append("end;");
            DbHelperOra.ExecuteSqlTran(strSql.ToString(), ServiceUtils.CreateOracleParameter(":REQUISITION_CD", OracleDbType.Varchar2, REQUISITION_CD),
                ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id));
            return true;
        }

        [WebMethod(EnableSession = true)]
        public bool DeleteLogProcess(string PROCESS_ID)
        {
            if (string.IsNullOrWhiteSpace(PROCESS_ID)) return false;
            //if (!new UserService().GetActiveUserRoles().Contains(ServiceEnums.UserRole.系统管理员))
            //    ServiceUtils.ThrowSoapException("没有权限！");
            Workflow.Controller.ValidateDelete();
            string server_filepath = string.Empty;
            DataTable del_file = new DataTable();
            del_file.Columns.Add("UPLOADID", Type.GetType("System.Decimal"));
            del_file.Columns.Add("SHA1", Type.GetType("System.String"));
            del_file.Columns.Add("MD5", Type.GetType("System.String"));
            del_file.Columns.Add("LENGTH", Type.GetType("System.Decimal"));
            del_file.Columns.Add("PATHID", Type.GetType("System.Decimal"));
            del_file.Columns.Add("PATHMAIN", Type.GetType("System.String"));

            StringBuilder strSql = new StringBuilder();
            strSql.Append("begin ");
            strSql.Append("delete from DM_LOG_PROCESS WHERE PROCESS_ID=:PROCESS_ID;");
            strSql.Append("delete from SYS_WORK_FLOW WHERE OBJ_ID=:PROCESS_ID and flow_type in(3,4,5);");

            var dt1 = DbHelperOra.Query("select distinct bb.UPLOADID from sys_processing_uploadfile aa,sys_file_upload bb where bb.fileid = aa.fileid and aa.process_id=" + PROCESS_ID).Tables[0];
            if (dt1 == null || dt1.Rows.Count < 1) return false;
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                var del_filepath = DbHelperOra.Query("select * from sys_upload where uploadid=" + dt1.Rows[i].FieldEx<decimal>("UPLOADID")).Tables[0];
                DataRow dr = del_file.NewRow();
                dr["UPLOADID"] = del_filepath.Rows[0].FieldEx<decimal>("UPLOADID");
                dr["SHA1"] = del_filepath.Rows[0].FieldEx<string>("SHA1");
                dr["MD5"] = del_filepath.Rows[0].FieldEx<string>("MD5");
                dr["LENGTH"] = del_filepath.Rows[0].FieldEx<decimal>("LENGTH");
                dr["PATHID"] = del_filepath.Rows[0].FieldEx<decimal>("PATHID");
                dr["PATHMAIN"] = del_filepath.Rows[0].FieldEx<string>("PATHMAIN");
                del_file.Rows.Add(dr);

                strSql.Append("delete from sys_upload WHERE UPLOADID=" + dt1.Rows[i].FieldEx<decimal>("UPLOADID") + ";");
            }
            //for (int j = 1; j < 6; j++)
            //{
            //    var dt2 = DbHelperOra.Query("select distinct bb.pathid from sys_processing_uploadfile aa,sys_file_upload bb where bb.fileid = aa.fileid and aa.FILETYPE=" + j + " and aa.process_id=" + PROCESS_ID).Tables[0];
            //    if (dt2 != null && dt2.Rows.Count == 1)
            //    {
            //        var dt3 = DbHelperOra.Query("select * from sys_file_upload where pathid=" + dt2.Rows[0].FieldEx<decimal>("pathid")).Tables[0];
            //        if (dt3 != null && dt3.Rows.Count == 1)
            //            strSql.Append("delete from sys_file_path WHERE pathid=" + dt2.Rows[0].FieldEx<decimal>("pathid") + ";");
            //    }
            //}

            strSql.Append("end;");
            DbHelperOra.ExecuteSqlTran(strSql.ToString(), ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, PROCESS_ID));
            for (int j = 0; j < del_file.Rows.Count; j++)
            {
                string sha1 = del_file.Rows[j]["SHA1"].ToString();
                string md5 = del_file.Rows[j]["MD5"].ToString();
                int length = Int32.Parse(del_file.Rows[j]["LENGTH"].ToString());
                int pathid = Int32.Parse(del_file.Rows[j]["PATHID"].ToString());
                string path = FileUpload.PathList.FirstOrDefault(o => o.id == pathid).name;
                path = path.Remove(path.LastIndexOf('\\') + 1);
                path += del_file.Rows[j]["PATHMAIN"].ToString();

                server_filepath = string.Format("{0}\\{1}-{2}-{3}", path, sha1, md5, length);

                try
                {
                    //删除服务器上文件
                    if (File.Exists(server_filepath))
                    {
                        File.Delete(server_filepath);
                        File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "LogFile\\DeleteLogProcess.txt", DateTime.Now + "：：路径:" + server_filepath + "\r\n");
                    }
                }
                catch (Exception ex)
                {
                    File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "LogFile\\DeleteLogProcess_Error.txt", DateTime.Now + "：：路径:" + server_filepath + ",异常信息：" + ex.Message + "\r\n");
                }
            }
            DbHelperOra.ExecuteSql("delete from sys_file_path where pathid not in(select pathid from sys_file_upload)");
            return true;
        }
        //删除井及关联数据
        [WebMethod(EnableSession = true)]
        public bool DeleteComWellBasic(string WELL_ID)
        {
            if (string.IsNullOrWhiteSpace(WELL_ID)) return false;
            Workflow.Controller.ValidateDelete();
            IDataParameter[] param = new IDataParameter[]{
                new OracleParameter("WellID",WELL_ID),
                new OracleParameter("p_cursor",OracleDbType.RefCursor,ParameterDirection.Output)
            };
            //执行存储过程
            var dt = DbHelperOra.RunProcedure("DEL_WELL_DATA", param, "p_cursor").Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var fileid = dt.Rows[i].FieldEx<decimal>("FILEID");
                    var uploadid = dt.Rows[i].FieldEx<decimal>("UPLOADID");
                    DbHelperOra.ExecuteSql("delete from sys_file_upload where fileid=:FILEID",
                        ServiceUtils.CreateOracleParameter(":FILEID", OracleDbType.Decimal, fileid));
                    var isExists = DbHelperOra.Exists("select count(1) from sys_file_upload where rownum<2 and uploadid=:UPLOADID",
                        ServiceUtils.CreateOracleParameter(":UPLOADID", OracleDbType.Decimal, uploadid));
                    if (isExists) continue;

                    var sha1 = dt.Rows[i].FieldEx<string>("SHA1");
                    var md5 = dt.Rows[i].FieldEx<string>("MD5");
                    var length = dt.Rows[i].FieldEx<decimal>("LENGTH");
                    var path = FileUpload.PathList.FirstOrDefault(o => o.id == dt.Rows[i].FieldEx<decimal>("PATHID")).name;
                    path = path.Remove(path.LastIndexOf('\\') + 1);
                    path += dt.Rows[i].FieldEx<string>("PATHMAIN");
                    var server_filepath = string.Format("{0}\\{1}-{2}-{3}", path, sha1, md5, length);
                    try
                    {
                        //删除服务器上文件
                        if (File.Exists(server_filepath))
                        {
                            File.Delete(server_filepath);
                            File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "LogFile\\delete_well.txt", DateTime.Now + "：：路径:" + server_filepath + "\r\n");
                        }
                    }
                    catch (Exception ex)
                    {
                        File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "LogFile\\delete_well_error.txt", DateTime.Now + "：：路径:" + server_filepath + ",异常信息：" + ex.Message + "\r\n");
                    }
                }
            }
            DbHelperOra.ExecuteSql("delete from sys_file_path where pathid not in(select pathid from sys_file_upload)");
            return true;
        }

        //删除取芯参数
        [WebMethod(EnableSession = true)]
        public bool DeleteLogCore(string CORE_ID)
        {
            if (string.IsNullOrWhiteSpace(CORE_ID))
            {
                return false;
            }
            string strsql = "delete from PRO_LOG_CORE where COREID=:CORE_ID";
            DbHelperOra.ExecuteSql(strsql, ServiceUtils.CreateOracleParameter(":CORE_ID", OracleDbType.Varchar2, CORE_ID));
            return true;
        }

        //删除井试油参数
        [WebMethod(EnableSession = true)]
        public bool DeleteLogTestOil(string TESTOIL_ID)
        {
            if (string.IsNullOrWhiteSpace(TESTOIL_ID))
            {
                return false;
            }
            string strsql = "delete from PRO_LOG_TESTOIL where TESTOILID=:TESTOIL_ID";
            DbHelperOra.ExecuteSql(strsql, ServiceUtils.CreateOracleParameter(":TESTOIL_ID", OracleDbType.Char, TESTOIL_ID));
            return true;
        }

        //删除钻井液参数
        [WebMethod(EnableSession = true)]
        public bool DeleteLogSlop(string MUD_ID)
        {
            if (string.IsNullOrWhiteSpace(MUD_ID))
            {
                return false;
            }
            string strsql = "delete from PRO_LOG_SLOP where MUDID=:MUD_ID";
            DbHelperOra.ExecuteSql(strsql, ServiceUtils.CreateOracleParameter(":MUD_ID", OracleDbType.Varchar2, MUD_ID));
            return true;
        }
        //删除生产参数
        [WebMethod(EnableSession = true)]
        public bool DeleteLogProduce(string WORK_ID)
        {
            if (string.IsNullOrWhiteSpace(WORK_ID))
            {
                return false;
            }
            string strsql = "delete from PRO_LOG_PRODUCE where WORKID=:WORK_ID";
            DbHelperOra.ExecuteSql(strsql, ServiceUtils.CreateOracleParameter(":WORK_ID", OracleDbType.Varchar2, WORK_ID));
            return true;
        }

        //删除套管程序
        [WebMethod(EnableSession = true)]
        public bool DeleteLogCasin(string CASIN_ID)
        {
            if (string.IsNullOrWhiteSpace(CASIN_ID))
            {
                return false;
            }
            string strsql = "delete from PRO_LOG_CASIN where CASINID=:CASIN_ID";
            DbHelperOra.ExecuteSql(strsql, ServiceUtils.CreateOracleParameter(":CASIN_ID", OracleDbType.Varchar2, CASIN_ID));
            return true;
        }

        //删除固井参数
        [WebMethod(EnableSession = true)]
        public bool DeleteLogCement(string CEMENT_ID)
        {
            if (string.IsNullOrWhiteSpace(CEMENT_ID))
            {
                return false;
            }
            string strsql = "delete from PRO_LOG_CEMENT where CEMENTID=:CEMENT_ID";
            DbHelperOra.ExecuteSql(strsql, ServiceUtils.CreateOracleParameter(":CEMENT_ID", OracleDbType.Varchar2, CEMENT_ID));
            return true;
        }

        //删除钻头程序参数
        [WebMethod(EnableSession = true)]
        public bool DeleteLogBitProgram(string BIT_ID)
        {
            if (string.IsNullOrWhiteSpace(BIT_ID))
            {
                return false;
            }
            string strsql = "delete from PRO_LOG_BIT_PROGRAM where BITID=:BIT_ID";
            DbHelperOra.ExecuteSql(strsql, ServiceUtils.CreateOracleParameter(":BIT_ID", OracleDbType.Varchar2, BIT_ID));
            return true;
        }

        //删除录井解释成果
        [WebMethod(EnableSession = true)]
        public bool DeleteLogLoggingInterpretation(string INTERPRETATION_CD)
        {
            if (string.IsNullOrWhiteSpace(INTERPRETATION_CD))
            {
                return false;
            }
            string strsql = "delete from DM_LOG_LOGGING_INTERPRETATION where INTERPRETATION_CD=:INTERPRETATION_CD";
            DbHelperOra.ExecuteSql(strsql, ServiceUtils.CreateOracleParameter(":INTERPRETATION_CD", OracleDbType.Varchar2, INTERPRETATION_CD));
            return true;
        }

        //删除地层分层数据
        [WebMethod(EnableSession = true)]
        public bool DeleteComBaseStrataLayer2(int SEQ_NO)
        {
            string strsql = "delete from COM_BASE_STRATA_LAYER2 where SEQ_NO=:SEQ_NO";
            DbHelperOra.ExecuteSql(strsql, ServiceUtils.CreateOracleParameter(":SEQ_NO", OracleDbType.Int32, SEQ_NO));
            return true;
        }


        [WebMethod(EnableSession = true)]
        public string[] GetFileViewExtensions()
        {
            return System.Configuration.ConfigurationManager.AppSettings["FileViewExtensions"].Split(';');
        }

        [WebMethod(EnableSession = true)]//
        public List<DAL.VIEW_REQUISITION_LIST> GetDataPushList(string well_job_name, string requisition_cd)
        {
            EntitiesA12 a12 = new EntitiesA12();
            var query = (from a in a12.LOG_OPS_JOB_ACTIVITY
                         where a.REQUISITION_ID != null
                         select a.REQUISITION_ID).ToArray();
            a12.Dispose();
            using (EntitiesLogging cj = new EntitiesLogging())
            {
                var q = from a in cj.VIEW_REQUISITION_LIST
                        where a.STATE == "归档完成" && !query.Contains(a.REQUISITION_CD)
                        select a;
                string tempStr;
                if (!string.IsNullOrWhiteSpace(well_job_name))
                {
                    tempStr = well_job_name.Trim().ToUpper();
                    q = q.Where(v => v.WELL_JOB_NAME.StartsWith(tempStr));
                }

                if (!string.IsNullOrWhiteSpace(requisition_cd))
                {
                    tempStr = requisition_cd.Trim().ToUpper();
                    q = q.Where(v => v.REQUISITION_CD.StartsWith(tempStr));
                }
                return q.Distinct().ToList();
            }
        }

        [WebMethod(EnableSession = true)]
        public bool VilidateDataPush(string process_name)
        {
            return DbHelperOra.Exists("select count(1) from DI_LOG_PUSH where rownum<2 and NAME=:PROCESS_NAME",
                ServiceUtils.CreateOracleParameter(":PROCESS_NAME", OracleDbType.Varchar2, process_name)
                );
        }

        [WebMethod(EnableSession = true)]
        public bool VilidateDataPushXY(string process_name)
        {
            return DbHelperOra.Exists("select count(1) from DI_LOG_PUSH_XY where rownum<2 and NAME=:PROCESS_NAME",
                ServiceUtils.CreateOracleParameter(":PROCESS_NAME", OracleDbType.Varchar2, process_name)
                );
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetDataPushListYTHPT(string well_job_name, string PART_UNITS, DateTime? LOG_START_TIME, string state, string state1, int page, out int total)
        {
            int pageSize = 50;
            var parameters = new List<OracleParameter>();
            var SQL = new StringBuilder();
            var whereSql = new StringBuilder("");
            SQL.Append("SELECT * ");
            SQL.Append("FROM ");
            SQL.Append("  (SELECT A.PROCESS_ID, B.WELL_JOB_NAME, ");
            SQL.Append("    A.PROCESS_NAME, B1.PART_UNITS,E1.LOG_START_TIME, ");
            SQL.Append("    CASE ");
            SQL.Append("      WHEN C.FLOW_STATE=1 ");
            SQL.Append("      THEN '等待审核' ");
            SQL.Append("      WHEN C.FLOW_STATE=2 ");
            SQL.Append("      THEN '归档完成' ");
            SQL.Append("      WHEN C.FLOW_STATE=1 ");
            SQL.Append("      THEN '审核未通过' ");
            SQL.Append("      ELSE '未归档' ");
            SQL.Append("    END STATE, ");
            SQL.Append("    CASE ");
            SQL.Append("      WHEN D.TARGET IS NULL ");
            SQL.Append("      THEN '未推送' ");
            SQL.Append("      ELSE '已推送' ");
            SQL.Append("    END TARGET, ");
            SQL.Append("   D.OPERATOR, ");
            SQL.Append("   D.LOG_TIME ");
            SQL.Append("  FROM DM_LOG_PROCESS A, ");
            SQL.Append("    COM_JOB_INFO B, ");
            SQL.Append("    Com_Well_Basic B1, ");
            SQL.Append("    (SELECT OBJ_ID,FLOW_STATE FROM SYS_WORK_FLOW_NOW WHERE FLOW_TYPE=5 ");
            SQL.Append("    ) C, ");
            SQL.Append("    DI_LOG_PUSH D,");
            SQL.Append("    DM_LOG_SOURCE_DATA E,");
            SQL.Append("    DM_LOG_WORK_ANING E1");
            SQL.Append("  WHERE A.DRILL_JOB_ID    =B.DRILL_JOB_ID ");
            SQL.Append("  AND C.OBJ_ID            =A.PROCESS_ID ");
            SQL.Append("  and B.well_id=B1.well_id ");
            SQL.Append("  And A.Process_Id=E.Log_Data_Id(+) ");
            SQL.Append("  and E.JOB_PLAN_CD=E1.JOB_PLAN_CD(+) ");
            //SQL.Append("  AND A.PROCESS_NAME NOT IN ");
            //SQL.Append("    (SELECT NAME FROM DI_LOG_PUSH WHERE TARGET='YTHPT'");
            //SQL.Append("    ) ");
            SQL.Append("  AND A.PROCESS_NAME=D.NAME(+) ");
            SQL.Append("  ) ");

            if (LOG_START_TIME != null)
            {
                whereSql.Append(" and LOG_START_TIME>=:LOG_START_TIME");
                parameters.Add(ServiceUtils.CreateOracleParameter(":LOG_START_TIME", OracleDbType.TimeStamp, LOG_START_TIME));
            }
            //if (!string.IsNullOrEmpty(QUERY_DATE))
            //{
            //    QUERY_DATE = DbHelperOra.GetSingle("select DATE_VALUE from  PKL_LOG_QUERY_DATE where QUREY_DATE='" + QUERY_DATE + "'").ToString();
            //    if (QUERY_DATE.ToUpper() != "ALL")
            //    {
            //        var arr = QUERY_DATE.Split('|');
            //        var date = Convert.ToDateTime(DbHelperOra.GetSingle("select FN_QUERY_DATE(:F,:N) from dual",
            //            ServiceUtils.CreateOracleParameter(":F", OracleDbType.Varchar2, arr[0]),
            //            ServiceUtils.CreateOracleParameter(":N", OracleDbType.Decimal, int.Parse(arr[1]))));
            //        if (int.Parse(arr[1]) == 0)
            //        {
            //            whereSql.Append(" and LOG_START_TIME<:QUERY_DATE_1");
            //            parameters.Add(ServiceUtils.CreateOracleParameter(":QUERY_DATE_1", OracleDbType.TimeStamp, date.AddDays(1)));
            //        }
            //        whereSql.Append(" and LOG_START_TIME>=:QUERY_DATE");
            //        parameters.Add(ServiceUtils.CreateOracleParameter(":QUERY_DATE", OracleDbType.TimeStamp, date));
            //    }
            //}

            if (!string.IsNullOrEmpty(well_job_name))
            {
                whereSql.Append(" and WELL_JOB_NAME like :WELL_JOB_NAME");
                parameters.Add(ServiceUtils.CreateOracleParameter(":WELL_JOB_NAME", OracleDbType.Varchar2, "%" + well_job_name + "%"));
            }

            if (!string.IsNullOrEmpty(state))
            {
                whereSql.Append(" and STATE=:STATE");
                parameters.Add(ServiceUtils.CreateOracleParameter(":STATE", OracleDbType.Varchar2, state));
            }

            if (!string.IsNullOrEmpty(state1))
            {
                whereSql.Append(" and TARGET=:TARGET");
                parameters.Add(ServiceUtils.CreateOracleParameter(":TARGET", OracleDbType.Varchar2, state1));
            }

            if (!string.IsNullOrEmpty(PART_UNITS))
            {
                whereSql.Append(" and PART_UNITS =:PART_UNITS");
                parameters.Add(ServiceUtils.CreateOracleParameter(":PART_UNITS", OracleDbType.Varchar2, "" + PART_UNITS.Trim() + ""));
            }


            if (whereSql.Length > 0)
            {
                whereSql.Remove(0, 4);
                whereSql.Insert(0, "where");
            }

            var finalSql = SQL.ToString() + whereSql.ToString() + " ORDER BY Process_Name";
            total = Convert.ToInt32(DbHelperOra.GetSingle("select count(*) from(" + finalSql + ")", parameters.ToArray()));
            var pageSql = "select * from(select ROWNUM RN,K.* from(" + finalSql + ") K where ROWNUM<=" + page * pageSize + ") where RN>=" + ((page - 1) * pageSize + 1);
            return DbHelperOra.Query(pageSql, parameters.ToArray());
            //return DbHelperOra.Query(SQL.ToString() + whereSql.ToString() + " ORDER BY Process_Name", parameters.ToArray());
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetDataPushListXY(string well_job_name, string PART_UNITS, DateTime? LOG_START_TIME, string state, string state1, int page, out int total)
        {
            int pageSize = 50;
            var parameters = new List<OracleParameter>();
            var SQL = new StringBuilder();
            var whereSql = new StringBuilder("");
            SQL.Append("SELECT * ");
            SQL.Append("FROM ");
            SQL.Append("  (SELECT A.PROCESS_ID, B.WELL_JOB_NAME, ");
            SQL.Append("    A.PROCESS_NAME, B1.PART_UNITS,E1.LOG_START_TIME, ");
            SQL.Append("    CASE ");
            SQL.Append("      WHEN C.FLOW_STATE=1 ");
            SQL.Append("      THEN '等待审核' ");
            SQL.Append("      WHEN C.FLOW_STATE=2 ");
            SQL.Append("      THEN '归档完成' ");
            SQL.Append("      WHEN C.FLOW_STATE=1 ");
            SQL.Append("      THEN '审核未通过' ");
            SQL.Append("      ELSE '未归档' ");
            SQL.Append("    END STATE, ");
            SQL.Append("    CASE ");
            SQL.Append("      WHEN D.TARGET IS NULL ");
            SQL.Append("      THEN '未推送' ");
            SQL.Append("      ELSE '已推送' ");
            SQL.Append("    END TARGET, ");
            SQL.Append("   D.OPERATOR, ");
            SQL.Append("   D.LOG_TIME ");
            SQL.Append("  FROM DM_LOG_PROCESS A, ");
            SQL.Append("    COM_JOB_INFO B, ");
            SQL.Append("    Com_Well_Basic B1, ");
            SQL.Append("    (SELECT OBJ_ID,FLOW_STATE FROM SYS_WORK_FLOW_NOW WHERE FLOW_TYPE=5 ");
            SQL.Append("    ) C, ");
            SQL.Append("    DI_LOG_PUSH_XY D,");
            SQL.Append("    DM_LOG_SOURCE_DATA E,");
            SQL.Append("    DM_LOG_WORK_ANING E1");
            SQL.Append("  WHERE A.DRILL_JOB_ID    =B.DRILL_JOB_ID ");
            SQL.Append("  AND C.OBJ_ID            =A.PROCESS_ID ");
            SQL.Append("  and B.well_id=B1.well_id ");
            SQL.Append("  And A.Process_Id=E.Log_Data_Id(+) ");
            SQL.Append("  and E.JOB_PLAN_CD=E1.JOB_PLAN_CD(+) ");
            //SQL.Append("  AND A.PROCESS_NAME NOT IN ");
            //SQL.Append("    (SELECT NAME FROM DI_LOG_PUSH WHERE TARGET='YTHPT'");
            //SQL.Append("    ) ");
            SQL.Append("  AND A.PROCESS_NAME=D.NAME(+) ");
            SQL.Append("  ) ");

            if (LOG_START_TIME != null)
            {
                whereSql.Append(" and LOG_START_TIME>=:LOG_START_TIME");
                parameters.Add(ServiceUtils.CreateOracleParameter(":LOG_START_TIME", OracleDbType.TimeStamp, LOG_START_TIME));
            }

            if (!string.IsNullOrEmpty(well_job_name))
            {
                whereSql.Append(" and WELL_JOB_NAME like :WELL_JOB_NAME");
                parameters.Add(ServiceUtils.CreateOracleParameter(":WELL_JOB_NAME", OracleDbType.Varchar2, "%" + well_job_name + "%"));
            }

            if (!string.IsNullOrEmpty(state))
            {
                whereSql.Append(" and STATE=:STATE");
                parameters.Add(ServiceUtils.CreateOracleParameter(":STATE", OracleDbType.Varchar2, state));
            }

            if (!string.IsNullOrEmpty(state1))
            {
                whereSql.Append(" and TARGET=:TARGET");
                parameters.Add(ServiceUtils.CreateOracleParameter(":TARGET", OracleDbType.Varchar2, state1));
            }

            if (!string.IsNullOrEmpty(PART_UNITS))
            {
                whereSql.Append(" and PART_UNITS =:PART_UNITS");
                parameters.Add(ServiceUtils.CreateOracleParameter(":PART_UNITS", OracleDbType.Varchar2, "" + PART_UNITS.Trim() + ""));
            }

            if (whereSql.Length > 0)
            {
                whereSql.Remove(0, 4);
                whereSql.Insert(0, "where");
            }

            var finalSql = SQL.ToString() + whereSql.ToString() + " ORDER BY Process_Name";
            total = Convert.ToInt32(DbHelperOra.GetSingle("select count(*) from(" + finalSql + ")", parameters.ToArray()));
            var pageSql = "select * from(select ROWNUM RN,K.* from(" + finalSql + ") K where ROWNUM<=" + page * pageSize + ") where RN>=" + ((page - 1) * pageSize + 1);
            return DbHelperOra.Query(pageSql, parameters.ToArray());
            //return DbHelperOra.Query(SQL.ToString() + whereSql.ToString() + " ORDER BY Process_Name", parameters.ToArray());
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetFileDownloadList(string Well_Job_Name, string Well_Struct_Unit_Name, string Part_Units, string Process_Name)
        {
            if (!new UserService().GetActiveUserRoles().Contains(ServiceEnums.UserRole.批量下载员))
                return null;
            var parameters = new List<OracleParameter>();
            var SQL = new StringBuilder();
            SQL.Append("SELECT 0 Choise, ");
            SQL.Append("  A.Well_Job_Name, ");
            SQL.Append("  A.Well_Struct_Unit_Name, ");
            SQL.Append("  A.Part_Units, ");
            SQL.Append("  A.Process_Name, ");
            SQL.Append("  A.Length, ");
            SQL.Append("  A.File_Count, ");
            SQL.Append("  A.Process_Id, ");
            SQL.Append("  CASE ");
            SQL.Append("    WHEN B.Flow_State = 1 ");
            SQL.Append("    THEN '等待审核' ");
            SQL.Append("    WHEN B.Flow_State = 2 ");
            SQL.Append("    THEN '归档完成' ");
            SQL.Append("    WHEN B.Flow_State = 3 ");
            SQL.Append("    THEN '审核未通过' ");
            SQL.Append("    ELSE '未归档' ");
            SQL.Append("  END State, ");
            SQL.Append("  C.COPY_DVD_DATE, ");
            SQL.Append("  C.DVD_NUMBER, ");
            SQL.Append("  C.STORAGE_TANK_NO ");
            SQL.Append("FROM ");
            SQL.Append("  (SELECT MIN(B.Process_Name) Process_Name, ");
            SQL.Append("    MIN(C.Well_Job_Name) Well_Job_Name, ");
            SQL.Append("    MIN(D.Well_Struct_Unit_Name) Well_Struct_Unit_Name, ");
            SQL.Append("    MIN(D.Part_Units) Part_Units, ");
            SQL.Append("    SUM(A2.Length) LENGTH, ");
            SQL.Append("    COUNT(*) File_Count, ");
            SQL.Append("    B.Process_Id ");
            SQL.Append("  FROM ");
            SQL.Append("    (SELECT Process_Id, ");
            SQL.Append("      Fileid ");
            SQL.Append("    FROM Sys_Processing_Uploadfile ");
            SQL.Append("    GROUP BY Process_Id, ");
            SQL.Append("      Fileid ");
            SQL.Append("    ) A, ");
            SQL.Append("    Sys_File_Upload A1, ");
            SQL.Append("    Sys_Upload A2, ");
            SQL.Append("    Dm_Log_Process B, ");
            SQL.Append("    Com_Job_Info C, ");
            SQL.Append("    Com_Well_Basic D ");
            SQL.Append("  WHERE A.Process_Id=B.Process_Id ");
            SQL.Append("  AND B.Drill_Job_Id=C.Drill_Job_Id ");
            SQL.Append("  AND C.Well_Id     =D.Well_Id ");
            SQL.Append("  AND A.Fileid      =A1.Fileid ");
            SQL.Append("  AND A1.Uploadid   =A2.Uploadid ");
            SQL.Append("  GROUP BY B.Process_Id ");
            SQL.Append("  ) A, ");
            SQL.Append("  Sys_Work_Flow_Now B, ");
            SQL.Append("  Dm_Log_DVD_Info C ");
            SQL.Append("WHERE A.Process_Id=B.Obj_Id ");
            SQL.Append("AND A.Process_Id=C.Process_Id(+) ");
            SQL.Append("AND B.Flow_Type   =5");
            if (!string.IsNullOrWhiteSpace(Well_Job_Name))
            {
                SQL.Append(" and a.Well_Job_Name like :Well_Job_Name");
                parameters.Add(ServiceUtils.CreateOracleParameter(":Well_Job_Name", OracleDbType.Varchar2, "%" + Well_Job_Name.Trim() + "%"));
            }
            if (!string.IsNullOrWhiteSpace(Well_Struct_Unit_Name))
            {
                SQL.Append(" and a.Well_Struct_Unit_Name like :Well_Struct_Unit_Name");
                parameters.Add(ServiceUtils.CreateOracleParameter(":Well_Struct_Unit_Name", OracleDbType.Varchar2, "%" + Well_Struct_Unit_Name.Trim() + "%"));
            }
            if (!string.IsNullOrWhiteSpace(Part_Units))
            {
                SQL.Append(" and a.Part_Units like :Part_Units");
                parameters.Add(ServiceUtils.CreateOracleParameter(":Part_Units", OracleDbType.Varchar2, "%" + Part_Units.Trim() + "%"));
            }
            if (!string.IsNullOrWhiteSpace(Process_Name))
            {
                SQL.Append(" and a.Process_Name like :Process_Name");
                parameters.Add(ServiceUtils.CreateOracleParameter(":Process_Name", OracleDbType.Varchar2, "%" + Process_Name.Trim() + "%"));
            }
            return DbHelperOra.Query(SQL.ToString(), parameters.ToArray());
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetFileDownloadDetails(string process_Id)
        {
            if (!new UserService().GetActiveUserRoles().Contains(ServiceEnums.UserRole.批量下载员))
                return null;
            var SQL = new StringBuilder();
            SQL.Append("SELECT MIN(A.Process_Upload_Id) Process_Upload_Id, ");
            SQL.Append("  MIN(B.Filename) Filename, ");
            SQL.Append("  MIN(C.Filepath) Filepath ");
            SQL.Append("FROM Sys_Processing_Uploadfile a, ");
            SQL.Append("  Sys_File_Upload b , ");
            SQL.Append("  Sys_File_Path c ");
            SQL.Append("WHERE A.Fileid  =B.Fileid ");
            SQL.Append("AND B.Pathid    =C.Pathid(+) ");
            SQL.Append("AND A.Process_Id=:Process_Id ");
            SQL.Append("GROUP BY A.Fileid");
            return DbHelperOra.Query(SQL.ToString(), ServiceUtils.CreateOracleParameter(":Process_Id", OracleDbType.Varchar2, process_Id));
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetDvdInfo(string process_id)
        {
            return DbHelperOra.Query("select * FROM DM_LOG_DVD_INFO where PROCESS_ID=:PROCESS_ID",
                ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id));
        }

        [WebMethod(EnableSession = true)]
        public int SaveDvdInfo(byte[] _model,string process_id)
        {
            var model = ModelHelper.DeserializeObject(_model) as Model.DM_LOG_DVD_INFO;
            List<OracleParameter> parameters = new List<OracleParameter>();
            var strsql = new StringBuilder();
            if (string.IsNullOrEmpty(model.PROCESS_ID))
            {
                strsql.Append("insert into DM_LOG_DVD_INFO(");
                strsql.Append("PROCESS_ID,PROCESS_NAME,DVD_DIR_NAME,DVD_NUMBER,STORAGE_TANK_NO,COPY_DVD_MAN,COPY_DVD_DATE,NOTE");
                strsql.Append(") values (");
                strsql.Append(":PROCESS_ID,:PROCESS_NAME,:DVD_DIR_NAME,:DVD_NUMBER,:STORAGE_TANK_NO,:COPY_DVD_MAN,:COPY_DVD_DATE,:NOTE");
                strsql.Append(")");
            }
            else
            {
                strsql.Append("update DM_LOG_DVD_INFO set ");
                strsql.Append("PROCESS_NAME=:PROCESS_NAME,");
                strsql.Append("DVD_DIR_NAME=:DVD_DIR_NAME,");
                strsql.Append("DVD_NUMBER=:DVD_NUMBER,");
                strsql.Append("STORAGE_TANK_NO=:STORAGE_TANK_NO,");
                strsql.Append("COPY_DVD_MAN=:COPY_DVD_MAN,");
                strsql.Append("COPY_DVD_DATE=:COPY_DVD_DATE,");
                strsql.Append("NOTE=:NOTE");
                strsql.Append(" where PROCESS_ID=:PROCESS_ID");
            }

            parameters.AddRange(new OracleParameter[]{
                ServiceUtils.CreateOracleParameter(":PROCESS_ID",OracleDbType.Varchar2,process_id),
                ServiceUtils.CreateOracleParameter(":PROCESS_NAME",OracleDbType.Varchar2,model.PROCESS_NAME),
                ServiceUtils.CreateOracleParameter(":DVD_DIR_NAME",OracleDbType.Varchar2,model.DVD_DIR_NAME),
                ServiceUtils.CreateOracleParameter(":DVD_NUMBER",OracleDbType.Varchar2,model.DVD_NUMBER),
                ServiceUtils.CreateOracleParameter(":STORAGE_TANK_NO",OracleDbType.Varchar2,model.STORAGE_TANK_NO),
                ServiceUtils.CreateOracleParameter(":COPY_DVD_MAN",OracleDbType.Varchar2,model.COPY_DVD_MAN),
                ServiceUtils.CreateOracleParameter(":COPY_DVD_DATE",OracleDbType.TimeStamp,model.COPY_DVD_DATE),
                ServiceUtils.CreateOracleParameter(":NOTE",OracleDbType.Varchar2,model.NOTE)
            });
            return DbHelperOra.ExecuteSql(strsql.ToString(), parameters.ToArray());
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetDvdInfoList(byte[] _searchModel,int page,out int total)
        {
            int pageSize = 50;
            var model = Utility.ModelHelper.DeserializeObject(_searchModel) as Model.DM_LOG_DVD_INFO;
            var parameters = new List<OracleParameter>();
            var sql = new StringBuilder();
            var whereSql = new StringBuilder();

            sql.Append("select a.process_id,");
            sql.Append("a.process_name,");
            sql.Append("b.dvd_dir_name,");
            sql.Append("b.dvd_number,");
            sql.Append("b.storage_tank_no,");
            sql.Append("b.copy_dvd_man,");
            sql.Append("b.copy_dvd_date,");
            sql.Append("b.note ");
            sql.Append("from ");
            sql.Append("DM_LOG_PROCESS a,");
            sql.Append("DM_LOG_DVD_INFO b ");
            sql.Append("where a.process_id=b.process_id(+)");
            if (model != null)
            {
                if (!string.IsNullOrEmpty(model.PROCESS_NAME))
                {
                    whereSql.Append(" and regexp_like(a.PROCESS_NAME,:PROCESS_NAME,'i')");
                    parameters.Add(ServiceUtils.CreateOracleParameter(":PROCESS_NAME", OracleDbType.Varchar2, model.PROCESS_NAME));
                }
                if (!string.IsNullOrEmpty(model.DVD_NUMBER))
                {
                    whereSql.Append(" and regexp_like(DVD_NUMBER,:DVD_NUMBER,'i')");
                    parameters.Add(ServiceUtils.CreateOracleParameter(":DVD_NUMBER", OracleDbType.Varchar2, model.DVD_NUMBER));
                }
                if (!string.IsNullOrEmpty(model.STORAGE_TANK_NO))
                {
                    whereSql.Append(" and regexp_like(STORAGE_TANK_NO,:STORAGE_TANK_NO,'i')");
                    parameters.Add(ServiceUtils.CreateOracleParameter(":STORAGE_TANK_NO", OracleDbType.Varchar2, model.STORAGE_TANK_NO));
                }
            }

            var finalSql = sql.ToString() + whereSql.ToString();
            total = Convert.ToInt32(DbHelperOra.GetSingle("select count(*) from(" + finalSql + ")", parameters.ToArray()));
            var pageSql = "select * from(select ROWNUM RN,K.* from(" + finalSql + ") K where ROWNUM<=" + page * pageSize + ") where RN>=" + ((page - 1) * pageSize + 1);
            return DbHelperOra.Query(pageSql, parameters.ToArray());
        }

        [WebMethod(EnableSession = true)]
        public bool UpdateDrillID(decimal process_id, string drill_Job_ID)
        {
            StringBuilder SQL = new StringBuilder();
            List<OracleParameter> parameters = new List<OracleParameter>();
            SQL.Append("begin UPDATE dm_log_process ");
            SQL.Append("SET Drill_Job_Id=:drill_job_id ");
            SQL.Append("WHERE Process_Id=:process_id;");

            SQL.Append("UPDATE Dm_Log_Task ");
            SQL.Append("SET Drill_Job_Id      =:drill_job_id ");
            SQL.Append("WHERE REQUISITION_CD IN ");
            SQL.Append("  (SELECT A.Requisition_Cd ");
            SQL.Append("  FROM dm_log_ops_plan a, ");
            SQL.Append("    Dm_Log_Source_Data b ");
            SQL.Append("  WHERE A.Job_Plan_Cd=B.Job_Plan_Cd ");
            SQL.Append("  AND B.Log_Data_Id  =:process_id ");
            SQL.Append("  );end;");
            if (DbHelperOra.ExecuteSqlTran(SQL.ToString(),
                ServiceUtils.CreateOracleParameter(":process_id", OracleDbType.Decimal, process_id),
                ServiceUtils.CreateOracleParameter(":drill_job_id", OracleDbType.Varchar2, drill_Job_ID)) > 0)
                return true;
            return false;
        }



        #region
        /// <summary>
        /// 测井库获取井信息
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public DataSet GetDataList_井名()
        {
            return DbHelperOra.Query("Select distinct COM_JOB_INFO.WELL_JOB_NAME,COM_WELL_BASIC.WELL_LEGAL_NAME WELL_LEGALNAME,COM_WELL_BASIC.WELL_NAME,Com_Job_Info.Drill_Job_Id JOB_ID From COM_JOB_INFO,COM_WELL_BASIC Where Com_Job_Info.Well_Id=COM_WELL_BASIC.Well_Id");
        }
        /// <summary>
        /// 通知单引用参数
        /// </summary>
        /// <param name="WELL_JOB_NAME">测井作业井名</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public DataSet GetData_通知单引用参数(string WELL_JOB_NAME)
        {
            var ds = new DataSet();
            DataTable dt1 = null, dt2 = null, dt3 = null, dt4 = null, dt5 = null, dt6 = null, dt7 = null;
            dt1 = DbHelperOra.Query("SELECT WELL_ID,WELL_TYPE,WELL_SORT,DRILL_JOB_ID,WELL_JOB_NAME FROM  COM_JOB_INFO where WELL_JOB_NAME=:WELL_JOB_NAME", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":WELL_JOB_NAME", OracleDbType.Varchar2, WELL_JOB_NAME) }).Tables[0];
            if (dt1 != null && dt1.Rows.Count > 0)
            {
                dt2 = DbHelperOra.Query("select WELL_LEGAL_NAME,PART_UNITS,SITE_ID,STRUCTURAL_LOCATION,WELL_STRUCT_UNIT_NAME,WELL_HEAD_FLANGE from COM_WELL_BASIC where WELL_ID=:WELL_ID", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":WELL_ID", OracleDbType.Varchar2, dt1.Rows[0][0].ToString()) }).Tables[0];
                dt3 = DbHelperOra.Query("select MD,MAX_WELL_DEVIATION,MAX_WELL_DEVIATION_MD from COM_WELLBORE_BASIC where WELL_ID=:WELL_ID", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":WELL_ID", OracleDbType.Varchar2, dt1.Rows[0][0].ToString()) }).Tables[0];
                dt2.TableName = "COM_WELL_BASIC";
                dt3.TableName = "COM_WELLBORE_BASIC";
                ds.Tables.Add(dt2.Copy());
                ds.Tables.Add(dt3.Copy());
            }
            dt1.TableName = "COM_JOB_INFO";

            dt4 = DbHelperOra.Query("SELECT PRO_LOG_BIT_PROGRAM.BIT_SIZE from COM_JOB_INFO,DM_LOG_TASK,PRO_LOG_BIT_PROGRAM where COM_JOB_INFO.DRILL_JOB_ID=DM_LOG_TASK.DRILL_JOB_ID and PRO_LOG_BIT_PROGRAM.REQUISITION_CD=DM_LOG_TASK.REQUISITION_CD and COM_JOB_INFO.WELL_JOB_NAME=:WELL_JOB_NAME Order By PRO_LOG_BIT_PROGRAM.Update_Date", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":WELL_JOB_NAME", OracleDbType.Varchar2, WELL_JOB_NAME) }).Tables[0];
            dt5 = DbHelperOra.Query("SELECT PRO_LOG_CASIN.PIPE_HEIGHT CASING_SETTING_DEPTH,PRO_LOG_CASIN.CASING_OUTSIZE,PRO_LOG_CASIN.PIPE_THICKNESS,PRO_LOG_CASIN.CASING_OUTSIZE-PRO_LOG_CASIN.PIPE_THICKNESS CASING_INNERER_DIAMETER from COM_JOB_INFO,DM_LOG_TASK,PRO_LOG_CASIN where COM_JOB_INFO.DRILL_JOB_ID=DM_LOG_TASK.DRILL_JOB_ID and PRO_LOG_CASIN.REQUISITION_CD=DM_LOG_TASK.REQUISITION_CD and COM_JOB_INFO.WELL_JOB_NAME=:WELL_JOB_NAME Order By PRO_LOG_CASIN.Update_Date", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":WELL_JOB_NAME", OracleDbType.Varchar2, WELL_JOB_NAME) }).Tables[0];
            dt6 = DbHelperOra.Query("SELECT PRO_LOG_PRODUCE.WATER_PRO_PER_DAY,PRO_LOG_PRODUCE.AIR_DAY_PRODUCTION,PRO_LOG_PRODUCE.CASING_PRESSURE,PRO_LOG_PRODUCE.OIL_PRESSURE,PRO_LOG_PRODUCE.WELLBOTTOM_PRESS from COM_JOB_INFO,DM_LOG_TASK,PRO_LOG_PRODUCE where COM_JOB_INFO.DRILL_JOB_ID=DM_LOG_TASK.DRILL_JOB_ID and PRO_LOG_PRODUCE.REQUISITION_CD=DM_LOG_TASK.REQUISITION_CD and COM_JOB_INFO.WELL_JOB_NAME=:WELL_JOB_NAME Order By PRO_LOG_PRODUCE.Update_Date", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":WELL_JOB_NAME", OracleDbType.Varchar2, WELL_JOB_NAME) }).Tables[0];
            dt7 = DbHelperOra.Query("SELECT PRO_LOG_SLOP.P_XN_MUD_DENSITY,PRO_LOG_SLOP.DRILL_FLU_VISC,PRO_LOG_SLOP.API_FILTER_LOSS from COM_JOB_INFO,DM_LOG_TASK,PRO_LOG_SLOP where COM_JOB_INFO.DRILL_JOB_ID=DM_LOG_TASK.DRILL_JOB_ID and PRO_LOG_SLOP.REQUISITION_CD=DM_LOG_TASK.REQUISITION_CD and COM_JOB_INFO.WELL_JOB_NAME=:WELL_JOB_NAME Order By PRO_LOG_SLOP.Update_Date", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":WELL_JOB_NAME", OracleDbType.Varchar2, WELL_JOB_NAME) }).Tables[0];
            dt4.TableName = "PRO_LOG_BIT_PROGRAM";
            dt5.TableName = "PRO_LOG_CASIN";
            dt6.TableName = "PRO_LOG_PRODUCE";
            dt7.TableName = "PRO_LOG_SLOP";

            ds.Tables.Add(dt1.Copy());
            ds.Tables.Add(dt4.Copy());
            ds.Tables.Add(dt5.Copy());
            ds.Tables.Add(dt6.Copy());
            ds.Tables.Add(dt7.Copy());


            return ds;
        }
        /// <summary>
        /// 计划任务书引用参数
        /// </summary>
        /// <param name="REQUISITION_CD">通知单号</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public DataSet GetData_计划任务书参数显示(string REQUISITION_CD)
        {
            var ds = new DataSet();
            DataTable dt1 = null, dt2 = null, dt3 = null, dt4 = null, dt5 = null, dt6 = null, dt7 = null;
            dt1 = DbHelperOra.Query("SELECT distinct COM_JOB_INFO.WELL_ID,COM_JOB_INFO.WELL_TYPE,COM_JOB_INFO.WELL_SORT,COM_JOB_INFO.DRILL_JOB_ID,COM_JOB_INFO.WELL_JOB_NAME FROM  COM_JOB_INFO,DM_LOG_TASK where COM_JOB_INFO.DRILL_JOB_ID=DM_LOG_TASK.DRILL_JOB_ID and DM_LOG_TASK.REQUISITION_CD=:REQUISITION_CD", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":REQUISITION_CD", OracleDbType.NVarchar2, REQUISITION_CD) }).Tables[0];
            if (dt1 != null && dt1.Rows.Count > 0)
            {
                dt2 = DbHelperOra.Query("select WELL_LEGAL_NAME,PART_UNITS,SITE_ID,STRUCTURAL_LOCATION,WELL_STRUCT_UNIT_NAME,WELL_HEAD_FLANGE from COM_WELL_BASIC where WELL_ID=:WELL_ID", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":WELL_ID", OracleDbType.Varchar2, dt1.Rows[0][0].ToString()) }).Tables[0];
                dt3 = DbHelperOra.Query("select MD,MAX_WELL_DEVIATION,MAX_WELL_DEVIATION_MD from COM_WELLBORE_BASIC where WELL_ID=:WELL_ID", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":WELL_ID", OracleDbType.Varchar2, dt1.Rows[0][0].ToString()) }).Tables[0];
                dt2.TableName = "COM_WELL_BASIC";
                dt3.TableName = "COM_WELLBORE_BASIC";
                ds.Tables.Add(dt2.Copy());
                ds.Tables.Add(dt3.Copy());
            }
            dt1.TableName = "COM_JOB_INFO";

            dt4 = DbHelperOra.Query("SELECT BIT_SIZE from PRO_LOG_BIT_PROGRAM where REQUISITION_CD=:REQUISITION_CD Order By PRO_LOG_BIT_PROGRAM.Update_Date", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":REQUISITION_CD", OracleDbType.NVarchar2, REQUISITION_CD) }).Tables[0];
            dt5 = DbHelperOra.Query("Select PIPE_HEIGHT CASING_SETTING_DEPTH,CASING_OUTSIZE,PIPE_THICKNESS,CASING_OUTSIZE-PIPE_THICKNESS CASING_INNERER_DIAMETER From Pro_Log_Casin where REQUISITION_CD=:REQUISITION_CD Order By Pro_Log_Casin.Update_Date", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":REQUISITION_CD", OracleDbType.NVarchar2, REQUISITION_CD) }).Tables[0];
            dt6 = DbHelperOra.Query("SELECT WATER_PRO_PER_DAY,AIR_DAY_PRODUCTION,CASING_PRESSURE,OIL_PRESSURE,WELLBOTTOM_PRESS from PRO_LOG_PRODUCE where REQUISITION_CD=:REQUISITION_CD Order By PRO_LOG_PRODUCE.Update_Date", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":REQUISITION_CD", OracleDbType.NVarchar2, REQUISITION_CD) }).Tables[0];
            dt7 = DbHelperOra.Query("SELECT P_XN_MUD_DENSITY,DRILL_FLU_VISC,API_FILTER_LOSS from PRO_LOG_SLOP where REQUISITION_CD=:REQUISITION_CD Order By PRO_LOG_SLOP.Update_Date", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":REQUISITION_CD", OracleDbType.NVarchar2, REQUISITION_CD) }).Tables[0];
            dt4.TableName = "PRO_LOG_BIT_PROGRAM";
            dt5.TableName = "PRO_LOG_CASIN";
            dt6.TableName = "PRO_LOG_PRODUCE";
            dt7.TableName = "PRO_LOG_SLOP";

            ds.Tables.Add(dt1.Copy());
            ds.Tables.Add(dt4.Copy());
            ds.Tables.Add(dt5.Copy());
            ds.Tables.Add(dt6.Copy());
            ds.Tables.Add(dt7.Copy());


            return ds;
        }
        /// <summary>
        /// 查询缓存表SYS_WELL_CACHE
        /// </summary>
        /// <param name="DRILL_JOB_ID">作业项目ID</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public DataSet GetList_SYS_WELL_CACHE_ID(string DRILL_JOB_ID)
        {
            if (string.IsNullOrWhiteSpace(DRILL_JOB_ID)) return null;
            return DbHelperOra.Query("SELECT * FROM  SYS_WELL_CACHE where DRILL_JOB_ID=:DRILL_JOB_ID", new OracleParameter[] { new OracleParameter(":DRILL_JOB_ID", DRILL_JOB_ID) });
        }
        /// <summary>
        /// 查询缓存表SYS_WELL_CACHE
        /// </summary>
        /// <param name="WELL_JOB_NAME">测井作业井名</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public DataSet GetList_SYS_WELL_CACHE_NAME(string WELL_JOB_NAME)
        {
            if (string.IsNullOrWhiteSpace(WELL_JOB_NAME)) return null;
            return DbHelperOra.Query("SELECT * FROM  SYS_WELL_CACHE where WELL_JOB_NAME=:WELL_JOB_NAME", new OracleParameter[] { new OracleParameter(":WELL_JOB_NAME", WELL_JOB_NAME) });
        }
        /// <summary>
        /// 查询缓存表SYS_WELL_CACHE
        /// </summary>
        /// <param name="REQUISITION_CD">通知单号</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public DataSet GetList_SYS_WELL_CACHE_TASK(string REQUISITION_CD)
        {
            if (string.IsNullOrWhiteSpace(REQUISITION_CD)) return null;
            return DbHelperOra.Query("SELECT SYS_WELL_CACHE.* FROM  SYS_WELL_CACHE,COM_JOB_INFO,DM_LOG_TASK where COM_JOB_INFO.DRILL_JOB_ID=DM_LOG_TASK.DRILL_JOB_ID and COM_JOB_INFO.WELL_JOB_NAME=SYS_WELL_CACHE.WELL_JOB_NAME and DM_LOG_TASK.REQUISITION_CD=:REQUISITION_CD", new OracleParameter[] { new OracleParameter(":REQUISITION_CD", REQUISITION_CD) });
        }
        /// <summary>
        /// 删除缓存数据
        /// </summary>
        [WebMethod(EnableSession = true)]
        public bool Delete_SYS_WELL_CACHE(string DRILL_JOB_ID)
        {
            if (DRILL_JOB_ID == null) return false;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from SYS_WELL_CACHE ");
            strSql.Append(" where DRILL_JOB_ID=:DRILL_JOB_ID ");
            OracleParameter[] parameters = { new OracleParameter(":DRILL_JOB_ID", OracleDbType.Varchar2, 45) };
            parameters[0].Value = DRILL_JOB_ID;

            DbHelperOra.connectionString = PubConstant.GetConnectionString("ConnectionString");
            if (DbHelperOra.ExecuteSql(strSql.ToString(), parameters) > 0) return true;
            return false;
        }
        /// <summary>
        /// 获取作业项目ID
        /// </summary>
        /// <param name="WELL_JOB_NAME"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public DataSet GetData_COMJOBINFO(string WELL_JOB_NAME)
        {
            return DbHelperOra.Query("select DRILL_JOB_ID from COM_JOB_INFO where WELL_JOB_NAME=:WELL_JOB_NAME", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":WELL_JOB_NAME", OracleDbType.Varchar2, WELL_JOB_NAME) });
        }
        /// <summary>
        /// 获取作业井名
        /// </summary>
        /// <param name="WELL_JOB_NAME"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public DataSet Get_WELLJOBNAME(string REQUISITION_CD)
        {
            return DbHelperOra.Query("select distinct COM_JOB_INFO.WELL_JOB_NAME from DM_LOG_TASK,COM_JOB_INFO where COM_JOB_INFO.DRILL_JOB_ID=DM_LOG_TASK.DRILL_JOB_ID and DM_LOG_TASK.REQUISITION_CD=:REQUISITION_CD", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":REQUISITION_CD", OracleDbType.Varchar2, REQUISITION_CD) });
        }
        #endregion

        #region 读取155库井基本信息
        /// <summary>
        /// A12获取井名
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public DataSet GetDataList_A12井名()
        {
            return DbHelperOraA12.Query("select distinct com_base_well.WELL_NAME,com_base_well.WELL_LEGALNAME,com_base_job.WELL_JOB_NAME,com_base_job.JOB_ID From com_base_job,com_base_well Where com_base_job.WELL_ID=com_base_well.WELL_ID");
        }
        /// <summary>
        /// A12作业项目com_base_job
        /// </summary>
        /// <param name="JOB_ID"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public DataSet GetData_A12作业项目(string JOB_ID)
        {
            return DbHelperOraA12.Query("select * from com_base_job where JOB_ID=:JOB_ID", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":JOB_ID", OracleDbType.Varchar2, JOB_ID) });
        }
        /// <summary>
        /// A12井com_base_well
        /// </summary>
        /// <param name="JOB_ID"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public DataSet GetData_A12井(string WELL_ID)
        {
            return DbHelperOraA12.Query("select * from com_base_well where WELL_ID=:WELL_ID", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":WELL_ID", OracleDbType.Varchar2, WELL_ID) });
        }
        /// <summary>
        /// A12井筒com_base_wellbore
        /// </summary>
        /// <param name="JOB_ID"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public DataSet GetData_A12井筒(string WELL_ID)
        {
            return DbHelperOraA12.Query("select * from com_base_wellbore where WELL_ID=:WELL_ID", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":WELL_ID", OracleDbType.Varchar2, WELL_ID) });
        }
        /// <summary>
        /// A12井身com_base_wellstructure
        /// </summary>
        /// <param name="JOB_ID"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public DataSet GetData_A12井身结构数据(string WELL_ID)
        {
            return DbHelperOraA12.Query("select * from com_base_wellstructure where WELL_ID=:WELL_ID", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":WELL_ID", OracleDbType.Varchar2, WELL_ID) });
        }
        /// <summary>
        /// A12获取套管下深com_base_casing_procedure
        /// </summary>
        /// <param name="JOB_ID"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public DataSet GetData_A12套管下深(string WELL_ID)
        {
            return DbHelperOraA12.Query("select CASING_SETTING_DEPTH,CASING_INNERER_DIAMETER from com_base_casing_procedure where WELL_ID=:WELL_ID order by CASING_INNERER_DIAMETER", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":WELL_ID", OracleDbType.Varchar2, WELL_ID) });
        }
        /// <summary>
        /// A12获取钻头尺寸drill_ops_bit_use
        /// </summary>
        /// <param name="JOB_ID"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public DataSet GetData_A12钻头尺寸(string WELL_ID)
        {
            return DbHelperOraA12.Query("select BIT_SIZE from drill_ops_bit_use where WELL_ID=:WELL_ID", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":WELL_ID", OracleDbType.Varchar2, WELL_ID) });
        }
        /// <summary>
        /// A12获取钻井液(密度、粘度、失水)drill_ops_fluid_performance
        /// </summary>
        /// <param name="JOB_ID"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public DataSet GetData_A12钻井液(string WELL_ID)
        {
            return DbHelperOraA12.Query("select FLUID_DENSITY,PV,API_FILTER_LOSS from drill_ops_fluid_performance where WELL_ID=:WELL_ID", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":WELL_ID", OracleDbType.Varchar2, WELL_ID) });
        }
        /// <summary>
        /// A12获取钻达井深drill_ops_job_interval
        /// </summary>
        /// <param name="JOB_ID"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public DataSet GetData_A12钻达井深(string WELL_ID)
        {
            return DbHelperOraA12.Query("select ACTUAL_END_MD from drill_ops_job_interval where WELL_ID=:WELL_ID", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":WELL_ID", OracleDbType.Varchar2, WELL_ID) });
        }
        /// <summary>
        /// 读取155库数据写入.9库缓存表中
        /// </summary>
        /// <param name="JOB_ID">作业项目ID</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public bool Save_SYS_WELL_DATA(string JOB_ID)
        {
            if (JOB_ID == null) return false;
            var or = GetList_SYS_WELL_CACHE_ID(JOB_ID).Tables[0];
            if (or != null && or.Rows.Count > 0) if (!Delete_SYS_WELL_CACHE(JOB_ID)) return false;
            var COM_BASE_JOB = Utility.ModelHandler<Model.COM_BASE_JOB>.FillModel(GetData_A12作业项目(JOB_ID));
            if (COM_BASE_JOB == null) return false;
            var COM_BASE_WELL = Utility.ModelHandler<Model.COM_BASE_WELL>.FillModel(GetData_A12井(COM_BASE_JOB[0].WELL_ID));
            var COM_BASE_WELLBORE = Utility.ModelHandler<Model.COM_BASE_WELLBORE>.FillModel(GetData_A12井筒(COM_BASE_JOB[0].WELL_ID));
            //var com_base_casing_procedure = GetData_A12套管下深(COM_BASE_JOB[0].WELL_ID).Tables[0];
            var drill_ops_bit_use = GetData_A12钻头尺寸(COM_BASE_JOB[0].WELL_ID).Tables[0];
            var drill_ops_fluid_performance = GetData_A12钻井液(COM_BASE_JOB[0].WELL_ID).Tables[0];
            var drill_ops_job_interval = GetData_A12钻达井深(COM_BASE_JOB[0].WELL_ID).Tables[0];

            Double? MAX_WELL_DEVIATION = null;
            Double? MAX_WELL_DEVIATION_MD = null;
            Double? CASING_SETTING_DEPTH = null;
            Double? CASING_INNERER_DIAMETER = null;
            Double? BIT_SIZE = null;
            Double? FLUID_DENSITY = null;
            Double? PV = null;
            Double? API_FILTER_LOSS = null;
            Double? ACTUAL_END_MD = null;
            string WELL_JOB_NAME = null;
            string WELL_NAME = null;
            string WELL_LEGALNAME = null;
            if (COM_BASE_JOB[0].WELL_JOB_NAME != null)
                WELL_JOB_NAME = COM_BASE_JOB[0].WELL_JOB_NAME.ToString().Replace("井", "");
            if (COM_BASE_WELL[0].WELL_NAME != null)
                WELL_NAME = COM_BASE_WELL[0].WELL_NAME.ToString().Replace("井", "");
            if (COM_BASE_WELL[0].WELL_LEGALNAME != null)
                WELL_LEGALNAME = COM_BASE_WELL[0].WELL_LEGALNAME.ToString().Replace("井", "");

            if (COM_BASE_WELLBORE != null)
            {
                MAX_WELL_DEVIATION = (Double)COM_BASE_WELLBORE[0].MAX_WELL_DEVIATION;
                MAX_WELL_DEVIATION_MD = (Double)COM_BASE_WELLBORE[0].MAX_WELL_DEVIATION_MD;
            }
            //if (com_base_casing_procedure != null && com_base_casing_procedure.Rows.Count > 0)
            //{
            //    if (com_base_casing_procedure.Rows[0][0].ToString() != "") CASING_SETTING_DEPTH = Convert.ToDouble(com_base_casing_procedure.Rows[0][0].ToString());
            //    if (com_base_casing_procedure.Rows[0][1].ToString() != "") CASING_INNERER_DIAMETER = Convert.ToDouble(com_base_casing_procedure.Rows[0][1].ToString());
            //}
            if (drill_ops_bit_use != null && drill_ops_bit_use.Rows.Count > 0) if (drill_ops_bit_use.Rows[0][0].ToString() != "") BIT_SIZE = Convert.ToDouble(drill_ops_bit_use.Rows[0][0].ToString());
            if (drill_ops_fluid_performance != null && drill_ops_fluid_performance.Rows.Count > 0)
            {
                if (drill_ops_fluid_performance.Rows[0][0].ToString() != "") FLUID_DENSITY = Convert.ToDouble(drill_ops_fluid_performance.Rows[0][0].ToString());
                if (drill_ops_fluid_performance.Rows[0][1].ToString() != "") PV = Convert.ToDouble(drill_ops_fluid_performance.Rows[0][1].ToString());
                if (drill_ops_fluid_performance.Rows[0][2].ToString() != "") API_FILTER_LOSS = Convert.ToDouble(drill_ops_fluid_performance.Rows[0][2].ToString());
            }
            if (drill_ops_job_interval != null && drill_ops_job_interval.Rows.Count > 0) if (drill_ops_job_interval.Rows[0][0].ToString() != "") ACTUAL_END_MD = Convert.ToDouble(drill_ops_job_interval.Rows[0][0].ToString());

            StringBuilder strSql = new StringBuilder();
            List<OracleParameter> parameters = new List<OracleParameter>();
            if (COM_BASE_JOB != null && COM_BASE_JOB.Count == 1)
            {
                strSql.Append("insert into SYS_WELL_CACHE(");
                strSql.Append("DRILL_JOB_ID,WELL_ID,WELL_JOB_NAME,WELL_TYPE,WELL_SORT,WELL_NAME,WELL_LEGAL_NAME,STRUCTURAL_LOCATION,PART_UNITS,MAX_WELL_DEVIATION,MAX_WELL_DEVIATION_MD,SITE_ID,BIT_SIZE,CASING_SETTING_DEPTH,P_XN_MUD_DENSITY,DRILL_FLU_VISC,API_FILTER_LOSS,MD,CASING_INNERER_DIAMETER");
                strSql.Append(") values (");
                strSql.Append(":DRILL_JOB_ID,:WELL_ID,:WELL_JOB_NAME,:WELL_TYPE,:WELL_SORT,:WELL_NAME,:WELL_LEGAL_NAME,:STRUCTURAL_LOCATION,:PART_UNITS,:MAX_WELL_DEVIATION,:MAX_WELL_DEVIATION_MD,:SITE_ID,:BIT_SIZE,:CASING_SETTING_DEPTH,:P_XN_MUD_DENSITY,:DRILL_FLU_VISC,:API_FILTER_LOSS,:MD,:CASING_INNERER_DIAMETER");
                strSql.Append(") ");
                parameters.Add(ServiceUtils.CreateOracleParameter(":DRILL_JOB_ID", OracleDbType.Varchar2, JOB_ID));
                parameters.Add(ServiceUtils.CreateOracleParameter(":WELL_ID", OracleDbType.Varchar2, COM_BASE_JOB[0].WELL_ID));
                parameters.AddRange(new OracleParameter[] {       
                        ServiceUtils.CreateOracleParameter(":WELL_JOB_NAME", OracleDbType.Varchar2, WELL_JOB_NAME) ,            
                        ServiceUtils.CreateOracleParameter(":WELL_TYPE", OracleDbType.Varchar2, COM_BASE_JOB[0].WELL_TYPE) ,            
                        ServiceUtils.CreateOracleParameter(":WELL_SORT", OracleDbType.Varchar2, COM_BASE_JOB[0].WELL_SORT) ,  
                        ServiceUtils.CreateOracleParameter(":WELL_NAME", OracleDbType.Varchar2, WELL_NAME) ,
                        ServiceUtils.CreateOracleParameter(":WELL_LEGAL_NAME", OracleDbType.Varchar2,WELL_LEGALNAME) ,            
                        ServiceUtils.CreateOracleParameter(":STRUCTURAL_LOCATION", OracleDbType.Varchar2, (COM_BASE_WELL==null?null:COM_BASE_WELL[0].STRUCTURE_LOCATION)) ,            
                        ServiceUtils.CreateOracleParameter(":PART_UNITS", OracleDbType.Varchar2, COM_BASE_JOB[0].OWNER_ORG_ID) ,                       
                        ServiceUtils.CreateOracleParameter(":MAX_WELL_DEVIATION", OracleDbType.Decimal, MAX_WELL_DEVIATION) ,
                        ServiceUtils.CreateOracleParameter(":MAX_WELL_DEVIATION_MD", OracleDbType.Decimal, MAX_WELL_DEVIATION_MD) ,          
                        ServiceUtils.CreateOracleParameter(":SITE_ID", OracleDbType.Varchar2, (COM_BASE_WELL==null?null:COM_BASE_WELL[0].WELL_BLOCK)) ,            
                        ServiceUtils.CreateOracleParameter(":BIT_SIZE", OracleDbType.Decimal, BIT_SIZE) ,            
                        ServiceUtils.CreateOracleParameter(":CASING_SETTING_DEPTH", OracleDbType.Decimal,CASING_SETTING_DEPTH) ,  
                        ServiceUtils.CreateOracleParameter(":P_XN_MUD_DENSITY", OracleDbType.Decimal, FLUID_DENSITY) ,
                        ServiceUtils.CreateOracleParameter(":DRILL_FLU_VISC", OracleDbType.Decimal, PV) ,            
                        ServiceUtils.CreateOracleParameter(":API_FILTER_LOSS", OracleDbType.Decimal, API_FILTER_LOSS) ,            
                        ServiceUtils.CreateOracleParameter(":MD", OracleDbType.Decimal, ACTUAL_END_MD) ,     //       
                        ServiceUtils.CreateOracleParameter(":CASING_INNERER_DIAMETER", OracleDbType.Decimal, CASING_INNERER_DIAMETER)
            });
                DbHelperOra.connectionString = PubConstant.GetConnectionString("ConnectionString");
                if (DbHelperOra.ExecuteSqlTran(strSql.ToString(), parameters.ToArray()) > 0)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 读取155库数据写入.9库井信息表中
        /// </summary>
        /// <param name="JOB_ID">作业项目ID</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public bool Save_ALL_WELL_DATA(string WELL_JOB_NAME)
        {
            if (WELL_JOB_NAME == null) return false;
            WELL_JOB_NAME = WELL_JOB_NAME.ToString().Replace("井", "");
            var data = GetList_SYS_WELL_CACHE_NAME(WELL_JOB_NAME).Tables[0];
            if (data == null || data.Rows.Count < 1) return false;

            var ds0 = GetData_A12作业项目(data.Rows[0]["DRILL_JOB_ID"].ToString());
            var COM_BASE_JOB = ds0.Tables[0].Rows.Count == 0 ? null : Utility.ModelHandler<Model.COM_BASE_JOB>.FillModel(ds0.Tables[0].Rows[0]);
            if (COM_BASE_JOB == null) return false;
            var ds1 = GetData_A12井(COM_BASE_JOB.WELL_ID);
            var ds2 = GetData_A12井筒(COM_BASE_JOB.WELL_ID);
            var ds3 = GetData_A12井身结构数据(COM_BASE_JOB.WELL_ID);
            var COM_BASE_WELL = ds1.Tables[0].Rows.Count == 0 ? null : Utility.ModelHandler<Model.COM_BASE_WELL>.FillModel(ds1.Tables[0]);
            var COM_BASE_WELLBORE = ds2.Tables[0].Rows.Count == 0 ? null : Utility.ModelHandler<Model.COM_BASE_WELLBORE>.FillModel(ds2.Tables[0]);
            var COM_BASE_WELLSTRUCTURE = ds3.Tables[0].Rows.Count == 0 ? null : Utility.ModelHandler<Model.COM_BASE_WELLSTRUCTURE>.FillModel(ds3.Tables[0]);

            string WELL_NAMEs = null;
            string PROJECT_NAME = null;
            if (COM_BASE_JOB.WELL_JOB_NAME != null)
                WELL_NAMEs = COM_BASE_JOB.WELL_JOB_NAME.ToString().Replace("井", "");
            if (COM_BASE_JOB.PROJECT_NAME != null)
                PROJECT_NAME = COM_BASE_JOB.PROJECT_NAME.ToString().Replace("井", "");

            StringBuilder strSql = new StringBuilder();
            var parameters = new List<OracleParameter>();
            strSql.Append("Begin ");
            strSql.Append("insert into COM_JOB_INFO(");
            strSql.Append("DRILL_JOB_ID,WELL_ID,WELL_JOB_NAME,ACTIVITY_NAME,FIELD,WELL_THE_MARKET,WELL_TYPE,WELL_SORT,TRUE_COMPLETION_FORMATION,COMPLETE_METHOD,JOB_PURPOSE");
            strSql.Append(") values (");
            strSql.Append("DRILL_JOB_ID_SEQ.nextval,WELL_ID_SEQUENCE.nextval,:WELL_JOB_NAME,:ACTIVITY_NAME,:FIELD,:WELL_THE_MARKET,:WELL_TYPE,:WELL_SORT,:TRUE_COMPLETION_FORMATION,:COMPLETE_METHOD,:JOB_PURPOSE");
            strSql.Append(") ;");
            parameters.AddRange(new OracleParameter[] {
                        //ServiceUtils.CreateOracleParameter(":WELL_ID", OracleDbType.Char, COM_BASE_JOB.WELL_ID) ,            
                        //ServiceUtils.CreateOracleParameter(":WELL_ID_A7", OracleDbType.Varchar2, COM_BASE_JOB.WELL_ID_A7) ,            
                        //ServiceUtils.CreateOracleParameter(":WELL_ID_A1", OracleDbType.Varchar2, COM_BASE_JOB.WELL_ID_A1) ,            
                        ServiceUtils.CreateOracleParameter(":WELL_JOB_NAME", OracleDbType.Varchar2,WELL_NAMEs) ,            
                        ServiceUtils.CreateOracleParameter(":ACTIVITY_NAME", OracleDbType.Varchar2,PROJECT_NAME) ,            
                        ServiceUtils.CreateOracleParameter(":FIELD", OracleDbType.Varchar2, COM_BASE_JOB.OWNER_ORG_ID) ,            
                        ServiceUtils.CreateOracleParameter(":WELL_THE_MARKET", OracleDbType.Varchar2, COM_BASE_JOB.WELL_THE_MARKET) ,            
                        ServiceUtils.CreateOracleParameter(":WELL_TYPE", OracleDbType.Varchar2, COM_BASE_JOB.WELL_TYPE) ,            
                        ServiceUtils.CreateOracleParameter(":WELL_SORT", OracleDbType.Varchar2, COM_BASE_JOB.WELL_SORT) ,            
                        ServiceUtils.CreateOracleParameter(":TRUE_COMPLETION_FORMATION", OracleDbType.Varchar2, COM_BASE_JOB.DESIGN_FINISH_FORMATION) ,            
                        ServiceUtils.CreateOracleParameter(":COMPLETE_METHOD", OracleDbType.Varchar2, COM_BASE_JOB.DESIGN_COMPLETE_METHOD) ,            
                        ServiceUtils.CreateOracleParameter(":JOB_PURPOSE", OracleDbType.Varchar2, COM_BASE_JOB.JOB_PURPOSE)               
            });
            if (COM_BASE_WELL != null)
            {
                string WELL_NAME = null;
                string WELL_LEGALNAME = null;
                if (COM_BASE_WELL[0].WELL_NAME != null)
                    WELL_NAME = COM_BASE_WELL[0].WELL_NAME.ToString().Replace("井", "");
                if (COM_BASE_WELL[0].WELL_LEGALNAME != null)
                    WELL_LEGALNAME = COM_BASE_WELL[0].WELL_LEGALNAME.ToString().Replace("井", "");

                WELL_LEGALNAME = WELL_LEGALNAME == null ? WELL_NAME : WELL_LEGALNAME;
                WELL_NAME = WELL_NAME == null ? WELL_LEGALNAME : WELL_NAME;

                strSql.Append("insert into COM_WELL_BASIC(");
                strSql.Append("WELL_ID,SITE_ID,WELL_NAME,WELL_LEGAL_NAME,WELL_UWI,WELL_STRUCT_UNIT_DESCRIPTION,WELL_FIELD_UNIT_NAME,WELL_STRUCT_UNIT_NAME,BEOG_LOCATION,STRUCTURAL_LOCATION,SURVEY_X_AXIS,SURVEY_Y_AXIS,RANGER_X_AXIS,RANGER_Y_AXIS,MAGNETIC_DECLINATION,WELL_HEAD_LONGITUDE,WELL_HEAD_LATITUDE,LOC_COUNTRY,LOC_STATE,P_LOC_CITY,LOC_COUNTY");
                strSql.Append(") values (");
                strSql.Append("WELL_ID_SEQUENCE.CURRVAL,:SITE_ID,:WELL_NAME,:WELL_LEGAL_NAME,:WELL_UWI,:WELL_STRUCT_UNIT_DESCRIPTION,:WELL_FIELD_UNIT_NAME,:WELL_STRUCT_UNIT_NAME,:BEOG_LOCATION,:STRUCTURAL_LOCATION,:SURVEY_X_AXIS,:SURVEY_Y_AXIS,:RANGER_X_AXIS,:RANGER_Y_AXIS,:MAGNETIC_DECLINATION,:WELL_HEAD_LONGITUDE,:WELL_HEAD_LATITUDE,:LOC_COUNTRY,:LOC_STATE,:P_LOC_CITY,:LOC_COUNTY");
                strSql.Append(") ;");
                parameters.AddRange(new OracleParameter[] {
                ServiceUtils.CreateOracleParameter(":SITE_ID", OracleDbType.Varchar2, COM_BASE_WELL[0].SITE_ID) ,            
                ServiceUtils.CreateOracleParameter(":WELL_NAME", OracleDbType.Varchar2, WELL_NAME) ,            
                ServiceUtils.CreateOracleParameter(":WELL_LEGAL_NAME", OracleDbType.Varchar2, WELL_LEGALNAME) ,            
                ServiceUtils.CreateOracleParameter(":WELL_UWI", OracleDbType.Varchar2, COM_BASE_WELL[0].WELL_UWI) ,            
                ServiceUtils.CreateOracleParameter(":WELL_STRUCT_UNIT_DESCRIPTION", OracleDbType.Varchar2, COM_BASE_WELL[0].STRUCTURE_DESCRIPTION) ,      
                ServiceUtils.CreateOracleParameter(":WELL_FIELD_UNIT_NAME", OracleDbType.Varchar2, COM_BASE_WELL[0].WELL_FIELD_UNIT_NAME) ,            
                ServiceUtils.CreateOracleParameter(":WELL_STRUCT_UNIT_NAME", OracleDbType.Varchar2, COM_BASE_WELL[0].STRUCT_CODE) ,            
                ServiceUtils.CreateOracleParameter(":BEOG_LOCATION", OracleDbType.Varchar2, COM_BASE_WELL[0].GEOGRAPHIC_LOCATION) ,            
                ServiceUtils.CreateOracleParameter(":STRUCTURAL_LOCATION", OracleDbType.Varchar2, COM_BASE_WELL[0].STRUCTURE_LOCATION) ,            
                //ServiceUtils.CreateOracleParameter(":TRAVERSE_LINE_LOCATION", OracleDbType.Varchar2, COM_BASE_WELL[0].TRAVERSE_LINE_LOCATION) ,            
                ServiceUtils.CreateOracleParameter(":SURVEY_X_AXIS", OracleDbType.Decimal, COM_BASE_WELL[0].DESIGN_X_AXIS) ,            
                ServiceUtils.CreateOracleParameter(":SURVEY_Y_AXIS", OracleDbType.Decimal, COM_BASE_WELL[0].DESIGN_Y_AXIS) ,            
                //ServiceUtils.CreateOracleParameter(":GROUND_ELEVATION", OracleDbType.Decimal, COM_BASE_WELL[0].GROUND_ELEVATION) ,            
                //ServiceUtils.CreateOracleParameter(":SYSTEM_DATUM_OFFSET", OracleDbType.Decimal, COM_BASE_WELL[0].SYSTEM_DATUM_OFFSET) ,            
                ServiceUtils.CreateOracleParameter(":RANGER_X_AXIS", OracleDbType.Decimal, COM_BASE_WELL[0].ACTUAL_X_AXIS) ,            
                ServiceUtils.CreateOracleParameter(":RANGER_Y_AXIS", OracleDbType.Decimal, COM_BASE_WELL[0].ACTUAL_Y_AXIS) ,            
                ServiceUtils.CreateOracleParameter(":MAGNETIC_DECLINATION", OracleDbType.Decimal, COM_BASE_WELL[0].ANGLE_DIP) ,            
                //ServiceUtils.CreateOracleParameter(":CASING_TOP_DEPTH", OracleDbType.Decimal, COM_BASE_WELL[0].CASING_TOP_DEPTH) ,            
                ServiceUtils.CreateOracleParameter(":WELL_HEAD_LONGITUDE", OracleDbType.Varchar2, COM_BASE_WELL[0].ACTUAL_LONGITUDE) ,            
                ServiceUtils.CreateOracleParameter(":WELL_HEAD_LATITUDE", OracleDbType.Varchar2, COM_BASE_WELL[0].ACTUAL_LATITUDE) ,            
                ServiceUtils.CreateOracleParameter(":LOC_COUNTRY", OracleDbType.Varchar2, COM_BASE_WELL[0].COUNTRY) ,            
                ServiceUtils.CreateOracleParameter(":LOC_STATE", OracleDbType.Varchar2, COM_BASE_WELL[0].LOC_STATE) ,            
                ServiceUtils.CreateOracleParameter(":P_LOC_CITY", OracleDbType.Varchar2, COM_BASE_WELL[0].CITY) ,        
                ServiceUtils.CreateOracleParameter(":LOC_COUNTY", OracleDbType.Varchar2, COM_BASE_WELL[0].LOC_COUNTY)
                //ServiceUtils.CreateOracleParameter(":DRILL_GEO_DES", OracleDbType.Blob, COM_BASE_WELL[0].DRILL_GEO_DES) ,     
                //ServiceUtils.CreateOracleParameter(":DRILL_ENG_DES", OracleDbType.Blob, COM_BASE_WELL[0].DRILL_ENG_DES) ,        
                //ServiceUtils.CreateOracleParameter(":PART_UNITS", OracleDbType.Varchar2, COM_BASE_WELL[0].PART_UNITS) ,        
                //ServiceUtils.CreateOracleParameter(":WELL_HEAD_FLANGE", OracleDbType.Char, COM_BASE_WELL[0].WELL_HEAD_FLANGE)
                });
            }
            if (COM_BASE_WELLBORE != null)
            {
                for (int i = 0; i < COM_BASE_WELLBORE.Count; i++)
                {
                    strSql.Append("insert into COM_WELLBORE_BASIC(");
                    strSql.Append("WELL_ID,WELLBORE_ID,P_WELLBORE_ID,WELLBORE_NAME,MAX_WELL_DEVIATION,MAX_WELL_DEVIATION_MD,DESIGN_VERTICAL_TVD,WELLBORE_PRODUCTION_DATE,DEFLECTION_POINT_MD,PLUGBACK_MD,PLUGBACK_TVD,TRUE_PLUGBACKTOTAL_DEPTH,DESIGN_HORIZON,DESIGN_MD,ACTUAL_HORIZON,BTM_X_COORDINATE,BTM_Y_COORDINATE");
                    strSql.Append(") values (");
                    strSql.Append("WELL_ID_SEQUENCE.CURRVAL,WELLBORE_ID_SEQUENCE.nextval,:P_WELLBORE_ID,:WELLBORE_NAME,:MAX_WELL_DEVIATION,:MAX_WELL_DEVIATION_MD,:DESIGN_VERTICAL_TVD,:WELLBORE_PRODUCTION_DATE,:DEFLECTION_POINT_MD,:PLUGBACK_MD,:PLUGBACK_TVD,:TRUE_PLUGBACKTOTAL_DEPTH,:DESIGN_HORIZON,:DESIGN_MD,:ACTUAL_HORIZON,:BTM_X_COORDINATE,:BTM_Y_COORDINATE");
                    strSql.Append(");");
                    parameters.AddRange(new OracleParameter[] {
                        //ServiceUtils.CreateOracleParameter(":WELL_ID", OracleDbType.Char, COM_BASE_WELLBORE[i].WELL_ID) ,            
                        //ServiceUtils.CreateOracleParameter(":WELLBORE_WUI", OracleDbType.Varchar2, COM_BASE_WELLBORE[i].WELLBORE_WUI) ,            
                        ServiceUtils.CreateOracleParameter(":P_WELLBORE_ID", OracleDbType.Char, COM_BASE_WELLBORE[i].PARENT_WELLBORE_ID) ,            
                        ServiceUtils.CreateOracleParameter(":WELLBORE_NAME", OracleDbType.Varchar2, COM_BASE_WELLBORE[i].WELLBORE_NAME) ,            
                        //ServiceUtils.CreateOracleParameter(":PURPOSE", OracleDbType.Varchar2, COM_BASE_WELLBORE[i].PURPOSE) ,            
                        ServiceUtils.CreateOracleParameter(":MAX_WELL_DEVIATION", OracleDbType.Decimal, COM_BASE_WELLBORE[i].MAX_WELL_DEVIATION) ,            
                        ServiceUtils.CreateOracleParameter(":MAX_WELL_DEVIATION_MD", OracleDbType.Decimal, COM_BASE_WELLBORE[i].MAX_WELL_DEVIATION_MD) ,            
                        ServiceUtils.CreateOracleParameter(":DESIGN_VERTICAL_TVD", OracleDbType.Decimal, COM_BASE_WELLBORE[i].AUTHORIZED_TVD) ,            
                        ServiceUtils.CreateOracleParameter(":WELLBORE_PRODUCTION_DATE", OracleDbType.Date, COM_BASE_WELLBORE[i].WELLBORE_PRODUCTION_DATE) ,        
                        ServiceUtils.CreateOracleParameter(":DEFLECTION_POINT_MD", OracleDbType.Decimal, COM_BASE_WELLBORE[i].DEFLECTION_POINT_MD) ,            
                        //ServiceUtils.CreateOracleParameter(":MD", OracleDbType.Decimal, COM_BASE_WELLBORE[i].MD) ,            
                        //ServiceUtils.CreateOracleParameter(":VERTICAL_WELL_TVD", OracleDbType.Decimal, COM_BASE_WELLBORE[i].VERTICAL_WELL_TVD) ,            
                        ServiceUtils.CreateOracleParameter(":PLUGBACK_MD", OracleDbType.Decimal, COM_BASE_WELLBORE[i].PLUGBACK_MD) ,            
                        ServiceUtils.CreateOracleParameter(":PLUGBACK_TVD", OracleDbType.Decimal, COM_BASE_WELLBORE[i].PLUGBACK_TVD) ,            
                        ServiceUtils.CreateOracleParameter(":TRUE_PLUGBACKTOTAL_DEPTH", OracleDbType.Decimal, COM_BASE_WELLBORE[i].ARTIFICIAL_WELLBOTTOM) ,          
                        ServiceUtils.CreateOracleParameter(":DESIGN_HORIZON", OracleDbType.Varchar2, COM_BASE_WELLBORE[i].DESIGN_FINISH_FORMATION) ,            
                        ServiceUtils.CreateOracleParameter(":DESIGN_MD", OracleDbType.Decimal, COM_BASE_WELLBORE[i].AUTHORIZED_MD) ,            
                        ServiceUtils.CreateOracleParameter(":ACTUAL_HORIZON", OracleDbType.Varchar2, COM_BASE_WELLBORE[i].ACTUAL_FINSH_FORMATION) ,            
                        ServiceUtils.CreateOracleParameter(":BTM_X_COORDINATE", OracleDbType.Decimal, COM_BASE_WELLBORE[i].OFFSET_EAST_BH) ,            
                        ServiceUtils.CreateOracleParameter(":BTM_Y_COORDINATE", OracleDbType.Decimal, COM_BASE_WELLBORE[i].OFFSET_NORTH_BH)  
            });
                    if (COM_BASE_WELLSTRUCTURE == null || COM_BASE_WELLSTRUCTURE.Count <= i)
                    {
                        strSql.Append("insert into COM_WELLSTRUCTURE_DATA(");
                        strSql.Append("WELLSTRUCTURE_ID,WELL_ID,WELLBORE_ID");
                        strSql.Append(") values (");
                        strSql.Append("WELLSTRUCTURE_ID_SEQUENCE.nextval,WELL_ID_SEQUENCE.CURRVAL,WELLBORE_ID_SEQUENCE.currval");
                        strSql.Append(");");
                    }
                    else
                    {
                        strSql.Append("insert into COM_WELLSTRUCTURE_DATA(");
                        strSql.Append("WELLSTRUCTURE_ID,WELL_ID,WELLBORE_ID,NO,CASING_NAME,MD,HORIZON,BORE_SIZE,STAGE_COLLAR1_DEPTH,STAGE_COLLAR2_DEPTH,FIRST_CEMENT_TOP,SECOND_CEMENT_TOP,THIRD_CEMENT_TOP,CEMENT_METHOD,ARTIFICIAL_WELL_BOTTOM_DEPTH");
                        strSql.Append(") values (");
                        strSql.Append("WELLSTRUCTURE_ID_SEQUENCE.nextval,WELL_ID_SEQUENCE.CURRVAL,WELLBORE_ID_SEQUENCE.currval,:NO,:CASING_NAME,:MD1,:HORIZON,:BORE_SIZE,:STAGE_COLLAR1_DEPTH,:STAGE_COLLAR2_DEPTH,:FIRST_CEMENT_TOP,:SECOND_CEMENT_TOP,:THIRD_CEMENT_TOP,:CEMENT_METHOD,:ARTIFICIAL_WELL_BOTTOM_DEPTH");
                        strSql.Append(");");
                        parameters.AddRange(new OracleParameter[] {
                        //ServiceUtils.CreateOracleParameter(":WELLSTRUCTURE_ID", OracleDbType.Varchar2, model2.WELLSTRUCTURE_ID) ,   
                        //ServiceUtils.CreateOracleParameter(":WELL_ID", OracleDbType.Char, model.WELL_ID) ,            
                        //ServiceUtils.CreateOracleParameter(":WELLBORE_ID", OracleDbType.Char, model.WELLBORE_ID) ,            
                        ServiceUtils.CreateOracleParameter(":NO", OracleDbType.Varchar2, COM_BASE_WELLSTRUCTURE[i].SPUDIN_NO) ,            
                        ServiceUtils.CreateOracleParameter(":CASING_NAME", OracleDbType.Varchar2, COM_BASE_WELLSTRUCTURE[i].CASING_NAME) ,            
                        ServiceUtils.CreateOracleParameter(":MD1", OracleDbType.Decimal, COM_BASE_WELLSTRUCTURE[i].WELL_DEPTH) ,            
                        ServiceUtils.CreateOracleParameter(":HORIZON", OracleDbType.Varchar2, COM_BASE_WELLSTRUCTURE[i].LAYER) ,            
                        ServiceUtils.CreateOracleParameter(":BORE_SIZE", OracleDbType.Decimal, COM_BASE_WELLSTRUCTURE[i].WELLBORE_SIZE) ,            
                        //ServiceUtils.CreateOracleParameter(":CASING_OD", OracleDbType.Decimal, model2.CASING_OD) ,            
                        //ServiceUtils.CreateOracleParameter(":BOINGTOOL_BOTTOM_DEPTH", OracleDbType.Decimal, model2.BOINGTOOL_BOTTOM_DEPTH) ,            
                        //ServiceUtils.CreateOracleParameter(":SETTING_DEPTH", OracleDbType.Decimal, model2.SETTING_DEPTH) ,            
                        //ServiceUtils.CreateOracleParameter(":CHOKE_COIL_DEPTH", OracleDbType.Decimal, model2.CHOKE_COIL_DEPTH) ,            
                        ServiceUtils.CreateOracleParameter(":STAGE_COLLAR1_DEPTH", OracleDbType.Decimal, COM_BASE_WELLSTRUCTURE[i].STAGE_COLLAR1_DEPTH) ,            
                        ServiceUtils.CreateOracleParameter(":STAGE_COLLAR2_DEPTH", OracleDbType.Decimal, COM_BASE_WELLSTRUCTURE[i].STAGE_COLLAR2_DEPTH) ,            
                        ServiceUtils.CreateOracleParameter(":FIRST_CEMENT_TOP", OracleDbType.Decimal, COM_BASE_WELLSTRUCTURE[i].CEMENT_TOP1) ,            
                        ServiceUtils.CreateOracleParameter(":SECOND_CEMENT_TOP", OracleDbType.Decimal, COM_BASE_WELLSTRUCTURE[i].CEMENT_TOP2) ,            
                        ServiceUtils.CreateOracleParameter(":THIRD_CEMENT_TOP", OracleDbType.Decimal, COM_BASE_WELLSTRUCTURE[i].CEMENT_TOP3) ,            
                        ServiceUtils.CreateOracleParameter(":CEMENT_METHOD", OracleDbType.Varchar2, COM_BASE_WELLSTRUCTURE[i].CEMENT_METHOD) ,            
                        ServiceUtils.CreateOracleParameter(":ARTIFICIAL_WELL_BOTTOM_DEPTH", OracleDbType.Decimal, COM_BASE_WELLSTRUCTURE[i].ARTIFICIAL_WELLBOTTOM)      
            });
                    }
                }
            }
            strSql.Append(" end;");
            if (DbHelperOra.ExecuteSql(strSql.ToString(), parameters.ToArray()) > 0) return true;
            return false;
        }
        #endregion
        #region 读取155库作业项目收集信息
        /// <summary>
        /// A12取心数据drill_ops_coring(取下入井深和取出井深)
        /// </summary>
        /// <param name="JOB_ID"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public DataSet GetData_A12取心数据(string WELL_JOB_NAME)
        {
            return DbHelperOraA12.Query("Select Distinct Drill_Ops_Coring.CORING_DATE,Drill_Ops_Coring.Tripin_Md,Drill_Ops_Coring.Coring_Depth From Com_Base_Job,Drill_Ops_Coring Where Com_Base_Job.Job_Id=Drill_Ops_Coring.Job_Id and com_base_job.WELL_JOB_NAME in(:WELL_JOB_NAME,:WELL_JOB_NAME1) order by Drill_Ops_Coring.CORING_DATE desc nulls last",
                new OracleParameter[] { ServiceUtils.CreateOracleParameter(":WELL_JOB_NAME", OracleDbType.Varchar2, WELL_JOB_NAME),
                ServiceUtils.CreateOracleParameter(":WELL_JOB_NAME1", OracleDbType.Varchar2, WELL_JOB_NAME+"井")});
        }
        /// <summary>
        /// A12钻井液全性能drill_ops_fluid_performance
        /// </summary>
        /// <param name="JOB_ID"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public DataSet GetData_A12钻井液全性能(string WELL_JOB_NAME)
        {
            return DbHelperOraA12.Query("Select Distinct drill_ops_fluid_performance.SAMPLING_TIME,drill_ops_fluid_performance.FLUID_SYSTEM,drill_ops_fluid_performance.FLUID_DENSITY,drill_ops_fluid_performance.PH,drill_ops_fluid_performance.PV,drill_ops_fluid_performance.MTO,drill_ops_fluid_performance.TOTAL_FILTRATE_SALINITY From Com_Base_Job,drill_ops_fluid_performance Where Com_Base_Job.Job_Id=drill_ops_fluid_performance.Job_Id and com_base_job.WELL_JOB_NAME in(:WELL_JOB_NAME,:WELL_JOB_NAME1) order by drill_ops_fluid_performance.SAMPLING_TIME desc nulls last",
                new OracleParameter[] { ServiceUtils.CreateOracleParameter(":WELL_JOB_NAME", OracleDbType.Varchar2, WELL_JOB_NAME),
                ServiceUtils.CreateOracleParameter(":WELL_JOB_NAME1", OracleDbType.Varchar2, WELL_JOB_NAME+"井")});
        }
        /// <summary>
        /// A12下套管记录drill_ops_strings_structure
        /// </summary>
        /// <param name="JOB_ID"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public DataSet GetData_A12下套管记录(string WELL_JOB_NAME)
        {
            return DbHelperOraA12.Query("Select Distinct drill_ops_strings_structure.* From Com_Base_Job,drill_ops_strings_structure Where Com_Base_Job.Job_Id=drill_ops_strings_structure.Job_Id and com_base_job.WELL_JOB_NAME in(:WELL_JOB_NAME,:WELL_JOB_NAME1) order by drill_ops_strings_structure.TRIPIN_DATE desc nulls last",
                new OracleParameter[] { ServiceUtils.CreateOracleParameter(":WELL_JOB_NAME", OracleDbType.Varchar2, WELL_JOB_NAME),
                ServiceUtils.CreateOracleParameter(":WELL_JOB_NAME1", OracleDbType.Varchar2, WELL_JOB_NAME+"井")});
        }
        /// <summary>
        /// A12钻头使用情况信息drill_ops_bit_use(取钻头尺寸和起出井深)
        /// </summary>
        /// <param name="JOB_ID"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public DataSet GetData_A12钻头使用情况信息(string WELL_JOB_NAME)
        {
            return DbHelperOraA12.Query("Select Distinct drill_ops_bit_use.BIT_TRIPIN_DATE,drill_ops_bit_use.BIT_SIZE,drill_ops_bit_use.TRIPOUT_MD From Com_Base_Job,drill_ops_bit_use Where Com_Base_Job.Job_Id=drill_ops_bit_use.Job_Id and com_base_job.WELL_JOB_NAME in(:WELL_JOB_NAME,:WELL_JOB_NAME1)  order by drill_ops_bit_use.BIT_TRIPIN_DATE desc nulls last",
                new OracleParameter[] { ServiceUtils.CreateOracleParameter(":WELL_JOB_NAME", OracleDbType.Varchar2, WELL_JOB_NAME),
                ServiceUtils.CreateOracleParameter(":WELL_JOB_NAME1", OracleDbType.Varchar2, WELL_JOB_NAME+"井")});
        }
        /// <summary>
        /// A12水泥浆信息drill_ops_cement_slurry(取水泥浆品种和水泥浆用量)
        /// </summary>
        /// <param name="JOB_ID"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public DataSet GetData_A12水泥浆信息(string WELL_JOB_NAME)
        {
            return DbHelperOraA12.Query("Select Distinct drill_ops_cement_slurry.CEMENT_TYPE,drill_ops_cement_slurry.CEMENT_USAGE From Com_Base_Job,drill_ops_cement_slurry Where Com_Base_Job.Job_Id=drill_ops_cement_slurry.Job_Id and com_base_job.WELL_JOB_NAME in(:WELL_JOB_NAME,:WELL_JOB_NAME1)",
                new OracleParameter[] { ServiceUtils.CreateOracleParameter(":WELL_JOB_NAME", OracleDbType.Varchar2, WELL_JOB_NAME),
                ServiceUtils.CreateOracleParameter(":WELL_JOB_NAME1", OracleDbType.Varchar2, WELL_JOB_NAME+"井")});
        }
        /// <summary>
        /// A12固井基础数据drill_ops_cement_info(取固井日期和固井结束日期)
        /// </summary>
        /// <param name="JOB_ID"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public DataSet GetData_A12固井基础数据(string WELL_JOB_NAME)
        {
            return DbHelperOraA12.Query("Select Distinct drill_ops_cement_info.CEMENT_DATE,drill_ops_cement_info.CEMENT_END_DATE From Com_Base_Job,drill_ops_cement_info Where Com_Base_Job.Job_Id=drill_ops_cement_info.Job_Id and com_base_job.WELL_JOB_NAME in(:WELL_JOB_NAME,:WELL_JOB_NAME1) order by drill_ops_cement_info.CEMENT_DATE desc nulls last",
                new OracleParameter[] { ServiceUtils.CreateOracleParameter(":WELL_JOB_NAME", OracleDbType.Varchar2, WELL_JOB_NAME),
                ServiceUtils.CreateOracleParameter(":WELL_JOB_NAME1", OracleDbType.Varchar2, WELL_JOB_NAME+"井")});
        }
        /// <summary>
        /// A12钻井施工基础数据drill_ops_construction(取油补距和套补距)
        /// </summary>
        /// <param name="JOB_ID"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public DataSet GetData_A12钻井施工基础数据(string WELL_JOB_NAME)
        {
            return DbHelperOraA12.Query("Select Distinct drill_ops_construction.DISTANCE_TUBING_BUSHING,drill_ops_construction.DISTANCE_CASING_BUSHING From Com_Base_Job,drill_ops_construction Where Com_Base_Job.Job_Id=drill_ops_construction.Job_Id and com_base_job.WELL_JOB_NAME in(:WELL_JOB_NAME,:WELL_JOB_NAME1)",
                new OracleParameter[] { ServiceUtils.CreateOracleParameter(":WELL_JOB_NAME", OracleDbType.Varchar2, WELL_JOB_NAME),
                ServiceUtils.CreateOracleParameter(":WELL_JOB_NAME1", OracleDbType.Varchar2, WELL_JOB_NAME+"井")});
        }
        [WebMethod(EnableSession = true)]
        public bool Save_ALL_JOB_DATA(string REQUISITION_CD, string JOB_PLAN_CD)
        {
            if (JOB_PLAN_CD == null) return false;
            var data = Get_WELLJOBNAME(REQUISITION_CD).Tables[0];
            if (data == null || data.Rows.Count < 1) return false;
            Save_A12取芯数据(REQUISITION_CD, JOB_PLAN_CD, data.Rows[0][0].ToString());
            Save_A12钻井液全性能(REQUISITION_CD, JOB_PLAN_CD, data.Rows[0][0].ToString());
            Save_A12套管参数(REQUISITION_CD, JOB_PLAN_CD, data.Rows[0][0].ToString());
            Save_A12钻头使用情况信息(REQUISITION_CD, JOB_PLAN_CD, data.Rows[0][0].ToString());
            Save_A12固井参数(REQUISITION_CD, JOB_PLAN_CD, data.Rows[0][0].ToString());
            #region
            //var PRO_LOG_CORE = GetData_A12取心数据(data.Rows[0][0].ToString()).Tables[0];
            //var PRO_LOG_SLOP = GetData_A12钻井液全性能(data.Rows[0][0].ToString()).Tables[0];
            //var PRO_LOG_CASIN = GetData_A12下套管记录(data.Rows[0][0].ToString()).Tables[0];
            //var PRO_LOG_BIT_PROGRAM = GetData_A12钻头使用情况信息(data.Rows[0][0].ToString()).Tables[0];
            //var PRO_LOG_CEMENT1 = GetData_A12水泥浆信息(data.Rows[0][0].ToString()).Tables[0];
            //var PRO_LOG_CEMENT2 = GetData_A12固井基础数据(data.Rows[0][0].ToString()).Tables[0];
            //var PRO_LOG_CEMENT3 = GetData_A12钻井施工基础数据(data.Rows[0][0].ToString()).Tables[0];

            //if (PRO_LOG_CORE.Rows.Count > 0 || PRO_LOG_SLOP.Rows.Count > 0 || PRO_LOG_CASIN.Rows.Count > 0 || PRO_LOG_BIT_PROGRAM.Rows.Count > 0 || PRO_LOG_CEMENT1.Rows.Count > 0 || PRO_LOG_CEMENT2.Rows.Count > 0 || PRO_LOG_CEMENT3.Rows.Count > 0)
            //{
            //    StringBuilder strSql = new StringBuilder();
            //    var parameters = new List<OracleParameter>();
            //    strSql.Append("Begin ");
            //    #region 取心数据
            //    if (PRO_LOG_CORE != null && PRO_LOG_CORE.Rows.Count > 0)
            //    {
            //        for (int i = 0; i < PRO_LOG_CORE.Rows.Count; i++)
            //        {
            //            Double? START_DEPTH = null;
            //            Double? END_DEPTH = null;
            //            if (PRO_LOG_CORE.Rows[i]["TRIPIN_MD"] != null && PRO_LOG_CORE.Rows[i]["TRIPIN_MD"].ToString() != "") START_DEPTH = Convert.ToDouble(PRO_LOG_CORE.Rows[i]["TRIPIN_MD"].ToString());
            //            if (PRO_LOG_CORE.Rows[i]["CORING_DEPTH"] != null && PRO_LOG_CORE.Rows[i]["CORING_DEPTH"].ToString() != "") END_DEPTH = Convert.ToDouble(PRO_LOG_CORE.Rows[i]["CORING_DEPTH"].ToString());

            //            strSql.Append("insert into PRO_LOG_CORE(");
            //            strSql.Append("COREID,JOB_PLAN_CD,REQUISITION_CD,UPDATE_DATE,START_DEPTH,END_DEPTH");
            //            strSql.Append(") values (");
            //            strSql.Append("COREID_SEQ.nextval,:JOB_PLAN_CD,:REQUISITION_CD,:UPDATE_DATE,:START_DEPTH,:END_DEPTH");
            //            strSql.Append(") ;");
            //            //parameters.Add(ServiceUtils.CreateOracleParameter(":COREID", OracleDbType.Varchar2, GetCOREID()));
            //            parameters.Add(ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, JOB_PLAN_CD));
            //            parameters.Add(ServiceUtils.CreateOracleParameter(":REQUISITION_CD", OracleDbType.Varchar2, REQUISITION_CD));
            //            parameters.AddRange(new OracleParameter[] {          
            //            ServiceUtils.CreateOracleParameter(":UPDATE_DATE", OracleDbType.Date, DateTime.Now) ,                 
            //            ServiceUtils.CreateOracleParameter(":START_DEPTH", OracleDbType.Decimal, START_DEPTH) ,            
            //            ServiceUtils.CreateOracleParameter(":END_DEPTH", OracleDbType.Decimal, END_DEPTH)
            //             });
            //        }
            //    }
            //    #endregion
            //    #region 钻井液全性能
            //    if (PRO_LOG_SLOP != null && PRO_LOG_SLOP.Rows.Count > 0)
            //    {
            //        for (int i = 0; i < PRO_LOG_SLOP.Rows.Count; i++)
            //        {
            //            Double? P_XN_MUD_DENSITY = null;
            //            Double? SLOP_PH = null;
            //            Double? DRILL_FLU_VISC = null;
            //            Double? SLOP_TEMPERATURE = null;
            //            Double? MUD_FILTRATE_SALINITY = null;
            //            if (PRO_LOG_SLOP.Rows[i]["FLUID_DENSITY"] != null && PRO_LOG_SLOP.Rows[i]["FLUID_DENSITY"].ToString() != "") P_XN_MUD_DENSITY = Convert.ToDouble(PRO_LOG_SLOP.Rows[i]["FLUID_DENSITY"].ToString());
            //            if (PRO_LOG_SLOP.Rows[i]["PH"] != null && PRO_LOG_SLOP.Rows[i]["PH"].ToString() != "") SLOP_PH = Convert.ToDouble(PRO_LOG_SLOP.Rows[i]["PH"].ToString());
            //            if (PRO_LOG_SLOP.Rows[i]["PV"] != null && PRO_LOG_SLOP.Rows[i]["PV"].ToString() != "") DRILL_FLU_VISC = Convert.ToDouble(PRO_LOG_SLOP.Rows[i]["PV"].ToString());
            //            if (PRO_LOG_SLOP.Rows[i]["MTO"] != null && PRO_LOG_SLOP.Rows[i]["MTO"].ToString() != "") SLOP_TEMPERATURE = Convert.ToDouble(PRO_LOG_SLOP.Rows[i]["MTO"].ToString());
            //            if (PRO_LOG_SLOP.Rows[i]["TOTAL_FILTRATE_SALINITY"] != null && PRO_LOG_SLOP.Rows[i]["TOTAL_FILTRATE_SALINITY"].ToString() != "") MUD_FILTRATE_SALINITY = Convert.ToDouble(PRO_LOG_SLOP.Rows[i]["TOTAL_FILTRATE_SALINITY"].ToString());

            //            strSql.Append("insert into PRO_LOG_SLOP(");
            //            strSql.Append("MUDID,JOB_PLAN_CD,REQUISITION_CD,UPDATE_DATE,JTLT,SLOP_PROPERTIES,P_XN_MUD_DENSITY,SLOP_PERSENT,SLOP_PH,DRILL_FLU_VISC,SLOP_TEMPERATURE,MUD_FILTRATE_SALINITY)");
            //            strSql.Append(" values (");
            //            strSql.Append("MUDID_SEQ.nextval,:JOB_PLAN_CD,:REQUISITION_CD,:UPDATE_DATE,:JTLT,:SLOP_PROPERTIES,:P_XN_MUD_DENSITY,:SLOP_PERSENT,:SLOP_PH,:DRILL_FLU_VISC,:SLOP_TEMPERATURE,:MUD_FILTRATE_SALINITY) ;");
            //            parameters.Add(ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, JOB_PLAN_CD));
            //            parameters.Add(ServiceUtils.CreateOracleParameter(":REQUISITION_CD", OracleDbType.NVarchar2, REQUISITION_CD));
            //            parameters.AddRange(new OracleParameter[] {          
            //            ServiceUtils.CreateOracleParameter(":UPDATE_DATE", OracleDbType.Date,DateTime.Now) ,            
            //            ServiceUtils.CreateOracleParameter(":JTLT", OracleDbType.Varchar2, PRO_LOG_SLOP.Rows[i]["FLUID_SYSTEM"].ToString()) ,            
            //            ServiceUtils.CreateOracleParameter(":SLOP_PROPERTIES", OracleDbType.Varchar2, PRO_LOG_SLOP.Rows[i]["FLUID_SYSTEM"].ToString()) ,            
            //            ServiceUtils.CreateOracleParameter(":P_XN_MUD_DENSITY", OracleDbType.Decimal, P_XN_MUD_DENSITY) ,            
            //            ServiceUtils.CreateOracleParameter(":SLOP_PERSENT", OracleDbType.Decimal, P_XN_MUD_DENSITY),
            //            ServiceUtils.CreateOracleParameter(":SLOP_PH", OracleDbType.Decimal, SLOP_PH) ,   
            //            ServiceUtils.CreateOracleParameter(":DRILL_FLU_VISC", OracleDbType.Decimal, DRILL_FLU_VISC) ,            
            //            ServiceUtils.CreateOracleParameter(":SLOP_TEMPERATURE", OracleDbType.Decimal, SLOP_TEMPERATURE) ,                      
            //            ServiceUtils.CreateOracleParameter(":MUD_FILTRATE_SALINITY", OracleDbType.Decimal, MUD_FILTRATE_SALINITY)
            //             });
            //        }
            //    }
            //    #endregion
            //    #region 套管参数
            //    if (PRO_LOG_CASIN != null && PRO_LOG_CASIN.Rows.Count > 0)
            //    {
            //        for (int i = 0; i < PRO_LOG_CASIN.Rows.Count; i++)
            //        {
            //            #region
            //            Double? CASING_OUTSIZE = null;
            //            Double? CASING_QUANTITY = null;
            //            Double? PIPE_THICKNESS = null;
            //            Double? P_XN_CASING_LENGTH = null;
            //            Double? ACCUMULATIVE_LENGTH = null;
            //            Double? RUNNING_BOTTOM_DEPTH = null;
            //            Double? SCREW_ON_TORQUE = null;
            //            Double? CENTERING_DEVICE_POSITION = null;
            //            Double? CENTERING_DEVICE_NO = null;
            //            Double? CENTERING_DEVICE_SIZE = null;
            //            Double? BASETUBE_BORESIZE = null;
            //            Double? BASETUBE_OUTSIZE = null;
            //            Double? BASETUBE_UNITNUMBER = null;
            //            Double? BASETUBE_SLOTWIDTH = null;
            //            Double? BASETUBE_POREDIAMETER = null;
            //            Double? SIEVETUBE_SLOTWIDTH = null;
            //            Double? SIEVETUBE_SLOTLENGTH = null;
            //            Double? PIPE_HEIGHT = null;
            //            if (PRO_LOG_CASIN.Rows[i]["CASING_OUTER_DIAMETER"] != null && PRO_LOG_CASIN.Rows[i]["CASING_OUTER_DIAMETER"].ToString() != "") CASING_OUTSIZE = Convert.ToDouble(PRO_LOG_CASIN.Rows[i]["CASING_OUTER_DIAMETER"].ToString());
            //            if (PRO_LOG_CASIN.Rows[i]["CASING_QUANTITY"] != null && PRO_LOG_CASIN.Rows[i]["CASING_QUANTITY"].ToString() != "") CASING_QUANTITY = Convert.ToDouble(PRO_LOG_CASIN.Rows[i]["CASING_QUANTITY"].ToString());
            //            if (PRO_LOG_CASIN.Rows[i]["CASING_THICKNESS"] != null && PRO_LOG_CASIN.Rows[i]["CASING_THICKNESS"].ToString() != "") PIPE_THICKNESS = Convert.ToDouble(PRO_LOG_CASIN.Rows[i]["CASING_THICKNESS"].ToString());
            //            if (PRO_LOG_CASIN.Rows[i]["CASING_LENGTH"] != null && PRO_LOG_CASIN.Rows[i]["CASING_LENGTH"].ToString() != "") P_XN_CASING_LENGTH = Convert.ToDouble(PRO_LOG_CASIN.Rows[i]["CASING_LENGTH"].ToString());
            //            if (PRO_LOG_CASIN.Rows[i]["ACCUMULATIVE_LENGTH"] != null && PRO_LOG_CASIN.Rows[i]["ACCUMULATIVE_LENGTH"].ToString() != "") ACCUMULATIVE_LENGTH = Convert.ToDouble(PRO_LOG_CASIN.Rows[i]["ACCUMULATIVE_LENGTH"].ToString());
            //            if (PRO_LOG_CASIN.Rows[i]["RUNNING_BOTTOM_DEPTH"] != null && PRO_LOG_CASIN.Rows[i]["RUNNING_BOTTOM_DEPTH"].ToString() != "") RUNNING_BOTTOM_DEPTH = Convert.ToDouble(PRO_LOG_CASIN.Rows[i]["RUNNING_BOTTOM_DEPTH"].ToString());
            //            if (PRO_LOG_CASIN.Rows[i]["SCREW_ON_TORQUE"] != null && PRO_LOG_CASIN.Rows[i]["SCREW_ON_TORQUE"].ToString() != "") SCREW_ON_TORQUE = Convert.ToDouble(PRO_LOG_CASIN.Rows[i]["SCREW_ON_TORQUE"].ToString());
            //            if (PRO_LOG_CASIN.Rows[i]["CENTERING_DEVICE_POSITION"] != null && PRO_LOG_CASIN.Rows[i]["CENTERING_DEVICE_POSITION"].ToString() != "") CENTERING_DEVICE_POSITION = Convert.ToDouble(PRO_LOG_CASIN.Rows[i]["CENTERING_DEVICE_POSITION"].ToString());
            //            if (PRO_LOG_CASIN.Rows[i]["CENTERING_DEVICE_NO"] != null && PRO_LOG_CASIN.Rows[i]["CENTERING_DEVICE_NO"].ToString() != "") CENTERING_DEVICE_NO = Convert.ToDouble(PRO_LOG_CASIN.Rows[i]["CENTERING_DEVICE_NO"].ToString());
            //            if (PRO_LOG_CASIN.Rows[i]["CENTERING_DEVICE_SIZE"] != null && PRO_LOG_CASIN.Rows[i]["CENTERING_DEVICE_SIZE"].ToString() != "") CENTERING_DEVICE_SIZE = Convert.ToDouble(PRO_LOG_CASIN.Rows[i]["CENTERING_DEVICE_SIZE"].ToString());
            //            if (PRO_LOG_CASIN.Rows[i]["BASETUBE_BORESIZE"] != null && PRO_LOG_CASIN.Rows[i]["BASETUBE_BORESIZE"].ToString() != "") BASETUBE_BORESIZE = Convert.ToDouble(PRO_LOG_CASIN.Rows[i]["BASETUBE_BORESIZE"].ToString());
            //            if (PRO_LOG_CASIN.Rows[i]["BASETUBE_OUTSIZE"] != null && PRO_LOG_CASIN.Rows[i]["BASETUBE_OUTSIZE"].ToString() != "") BASETUBE_OUTSIZE = Convert.ToDouble(PRO_LOG_CASIN.Rows[i]["BASETUBE_OUTSIZE"].ToString());
            //            if (PRO_LOG_CASIN.Rows[i]["BASETUBE_UNITNUMBER"] != null && PRO_LOG_CASIN.Rows[i]["BASETUBE_UNITNUMBER"].ToString() != "") BASETUBE_UNITNUMBER = Convert.ToDouble(PRO_LOG_CASIN.Rows[i]["BASETUBE_UNITNUMBER"].ToString());
            //            if (PRO_LOG_CASIN.Rows[i]["BASETUBE_SLOTWIDTH"] != null && PRO_LOG_CASIN.Rows[i]["BASETUBE_SLOTWIDTH"].ToString() != "") BASETUBE_SLOTWIDTH = Convert.ToDouble(PRO_LOG_CASIN.Rows[i]["BASETUBE_SLOTWIDTH"].ToString());
            //            if (PRO_LOG_CASIN.Rows[i]["BASETUBE_POREDIAMETER"] != null && PRO_LOG_CASIN.Rows[i]["BASETUBE_POREDIAMETER"].ToString() != "") BASETUBE_POREDIAMETER = Convert.ToDouble(PRO_LOG_CASIN.Rows[i]["BASETUBE_POREDIAMETER"].ToString());
            //            if (PRO_LOG_CASIN.Rows[i]["SIEVETUBE_SLOTWIDTH"] != null && PRO_LOG_CASIN.Rows[i]["SIEVETUBE_SLOTWIDTH"].ToString() != "") SIEVETUBE_SLOTWIDTH = Convert.ToDouble(PRO_LOG_CASIN.Rows[i]["SIEVETUBE_SLOTWIDTH"].ToString());
            //            if (PRO_LOG_CASIN.Rows[i]["SIEVETUBE_SLOTLENGTH"] != null && PRO_LOG_CASIN.Rows[i]["SIEVETUBE_SLOTLENGTH"].ToString() != "") SIEVETUBE_SLOTLENGTH = Convert.ToDouble(PRO_LOG_CASIN.Rows[i]["SIEVETUBE_SLOTLENGTH"].ToString());
            //            if (PRO_LOG_CASIN.Rows[i]["RUNNING_BOTTOM_DEPTH"] != null && PRO_LOG_CASIN.Rows[i]["RUNNING_BOTTOM_DEPTH"].ToString() != "") PIPE_HEIGHT = Convert.ToDouble(PRO_LOG_CASIN.Rows[i]["RUNNING_BOTTOM_DEPTH"].ToString());
            //            #endregion
            //            strSql.Append("insert into PRO_LOG_CASIN(");
            //            strSql.Append("CASINID,JOB_PLAN_CD,REQUISITION_CD,UPDATE_DATE,CASING_NAME,CASING_TYPE,CASING_OUTSIZE,CASING_QUANTITY,CASING_THREAD_TYPE,CASING_GRADE,PIPE_THICKNESS,P_XN_CASING_LENGTH,ACCUMULATIVE_LENGTH,RUNNING_BOTTOM_DEPTH,SCREW_ON_TORQUE,CENTERING_DEVICE_POSITION,CENTERING_DEVICE_NO,CENTERING_DEVICE_TYPE,CENTERING_DEVICE_SIZE,BASETUBE_BORESIZE,BASETUBE_OUTSIZE,BASETUBE_UNITNUMBER,BASETUBE_TYPE,BASETUBE_SLOTWIDTH,BASETUBE_POREDIAMETER,SIEVETUBE_SLOTWIDTH,SIEVETUBE_SLOTLENGTH,PIPE_HEIGHT");
            //            strSql.Append(") values (");
            //            strSql.Append("CASINID_SEQ.nextval,:JOB_PLAN_CD,:REQUISITION_CD,:UPDATE_DATE,:CASING_NAME,:CASING_TYPE,:CASING_OUTSIZE,:CASING_QUANTITY,:CASING_THREAD_TYPE,:CASING_GRADE,:PIPE_THICKNESS,:P_XN_CASING_LENGTH,:ACCUMULATIVE_LENGTH,:RUNNING_BOTTOM_DEPTH,:SCREW_ON_TORQUE,:CENTERING_DEVICE_POSITION,:CENTERING_DEVICE_NO,:CENTERING_DEVICE_TYPE,:CENTERING_DEVICE_SIZE,:BASETUBE_BORESIZE,:BASETUBE_OUTSIZE,:BASETUBE_UNITNUMBER,:BASETUBE_TYPE,:BASETUBE_SLOTWIDTH,:BASETUBE_POREDIAMETER,:SIEVETUBE_SLOTWIDTH,:SIEVETUBE_SLOTLENGTH,:PIPE_HEIGHT");
            //            strSql.Append(") ;");
            //            parameters.Add(ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, JOB_PLAN_CD));
            //            parameters.Add(ServiceUtils.CreateOracleParameter(":REQUISITION_CD", OracleDbType.NVarchar2, REQUISITION_CD));

            //            parameters.AddRange(new OracleParameter[] {          
            //            ServiceUtils.CreateOracleParameter(":UPDATE_DATE", OracleDbType.Date, DateTime.Now) ,            
            //            ServiceUtils.CreateOracleParameter(":CASING_NAME", OracleDbType.Varchar2, PRO_LOG_CASIN.Rows[i]["CASING_NAME"].ToString()) ,            
            //            ServiceUtils.CreateOracleParameter(":CASING_TYPE", OracleDbType.Varchar2, PRO_LOG_CASIN.Rows[i]["BASETUBE_TYPE"].ToString()) ,
            //            ServiceUtils.CreateOracleParameter(":CASING_OUTSIZE", OracleDbType.Decimal, CASING_OUTSIZE),
            //            ServiceUtils.CreateOracleParameter(":CASING_QUANTITY", OracleDbType.Decimal, CASING_QUANTITY) , 
            //ServiceUtils.CreateOracleParameter(":CASING_THREAD_TYPE", OracleDbType.Varchar2,PRO_LOG_CASIN.Rows[i]["CASING_THREAD_TYPE"].ToString()) ,            
            //            ServiceUtils.CreateOracleParameter(":CASING_GRADE", OracleDbType.Varchar2, PRO_LOG_CASIN.Rows[i]["CASING_GRADE"].ToString()) ,            
            //            ServiceUtils.CreateOracleParameter(":PIPE_THICKNESS", OracleDbType.Decimal, PIPE_THICKNESS) ,    
            //             ServiceUtils.CreateOracleParameter(":P_XN_CASING_LENGTH", OracleDbType.Decimal, P_XN_CASING_LENGTH) ,            
            //            ServiceUtils.CreateOracleParameter(":ACCUMULATIVE_LENGTH", OracleDbType.Decimal, ACCUMULATIVE_LENGTH) ,            
            //            ServiceUtils.CreateOracleParameter(":RUNNING_BOTTOM_DEPTH", OracleDbType.Decimal, RUNNING_BOTTOM_DEPTH) ,    
            //             ServiceUtils.CreateOracleParameter(":SCREW_ON_TORQUE", OracleDbType.Decimal, SCREW_ON_TORQUE) ,            
            //            ServiceUtils.CreateOracleParameter(":CENTERING_DEVICE_POSITION", OracleDbType.Decimal, CENTERING_DEVICE_POSITION) ,            
            //            ServiceUtils.CreateOracleParameter(":CENTERING_DEVICE_NO", OracleDbType.Decimal, CENTERING_DEVICE_NO) ,    
            //             ServiceUtils.CreateOracleParameter(":CENTERING_DEVICE_TYPE", OracleDbType.Varchar2, PRO_LOG_CASIN.Rows[i]["CENTERING_DEVICE_TYPE"].ToString()) ,            
            //            ServiceUtils.CreateOracleParameter(":CENTERING_DEVICE_SIZE", OracleDbType.Decimal, CENTERING_DEVICE_SIZE) ,            
            //            ServiceUtils.CreateOracleParameter(":BASETUBE_BORESIZE", OracleDbType.Decimal, BASETUBE_BORESIZE) ,    
            //            ServiceUtils.CreateOracleParameter(":BASETUBE_OUTSIZE", OracleDbType.Decimal, BASETUBE_OUTSIZE) ,            
            //            ServiceUtils.CreateOracleParameter(":BASETUBE_UNITNUMBER", OracleDbType.Decimal, BASETUBE_UNITNUMBER) ,            
            //            ServiceUtils.CreateOracleParameter(":BASETUBE_TYPE", OracleDbType.Varchar2, PRO_LOG_CASIN.Rows[i]["BASETUBE_TYPE"].ToString()) ,    
            //            ServiceUtils.CreateOracleParameter(":BASETUBE_SLOTWIDTH", OracleDbType.Decimal, BASETUBE_SLOTWIDTH) ,            
            //            ServiceUtils.CreateOracleParameter(":BASETUBE_POREDIAMETER", OracleDbType.Decimal, BASETUBE_POREDIAMETER) ,            
            //            ServiceUtils.CreateOracleParameter(":SIEVETUBE_SLOTWIDTH", OracleDbType.Decimal, SIEVETUBE_SLOTWIDTH) ,    
            //            ServiceUtils.CreateOracleParameter(":SIEVETUBE_SLOTLENGTH", OracleDbType.Decimal, SIEVETUBE_SLOTLENGTH) ,            
            //            ServiceUtils.CreateOracleParameter(":PIPE_HEIGHT", OracleDbType.Decimal, PIPE_HEIGHT)    
            //});
            //        }
            //    }
            //    #endregion
            //    #region 钻头使用情况信息
            //    if (PRO_LOG_BIT_PROGRAM != null && PRO_LOG_BIT_PROGRAM.Rows.Count > 0)
            //    {
            //        for (int i = 0; i < PRO_LOG_BIT_PROGRAM.Rows.Count; i++)
            //        {
            //            Double? BIT_SIZE = null;
            //            Double? BIT_DEP = null;
            //            if (PRO_LOG_BIT_PROGRAM.Rows[i]["BIT_SIZE"] != null && PRO_LOG_BIT_PROGRAM.Rows[i]["BIT_SIZE"].ToString() != "") BIT_SIZE = Convert.ToDouble(PRO_LOG_BIT_PROGRAM.Rows[i]["BIT_SIZE"].ToString());
            //            if (PRO_LOG_BIT_PROGRAM.Rows[i]["TRIPOUT_MD"] != null && PRO_LOG_BIT_PROGRAM.Rows[i]["TRIPOUT_MD"].ToString() != "") BIT_DEP = Convert.ToDouble(PRO_LOG_BIT_PROGRAM.Rows[i]["TRIPOUT_MD"].ToString());

            //            strSql.Append("insert into PRO_LOG_BIT_PROGRAM(");
            //            strSql.Append("BITID,JOB_PLAN_CD,REQUISITION_CD,UPDATE_DATE,BIT_SIZE,BIT_DEP,NOTE)");
            //            strSql.Append(" values (");
            //            strSql.Append("BITID_SEQ.nextval,:JOB_PLAN_CD,:REQUISITION_CD,:UPDATE_DATE,:BIT_SIZE,:BIT_DEP,:NOTE) ;");
            //            parameters.Add(ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, JOB_PLAN_CD));
            //            parameters.Add(ServiceUtils.CreateOracleParameter(":REQUISITION_CD", OracleDbType.NVarchar2, REQUISITION_CD));
            //            parameters.AddRange(new OracleParameter[] {          
            //            ServiceUtils.CreateOracleParameter(":UPDATE_DATE", OracleDbType.Date, DateTime.Now) ,            
            //            ServiceUtils.CreateOracleParameter(":BIT_SIZE", OracleDbType.Decimal, BIT_SIZE) ,            
            //            ServiceUtils.CreateOracleParameter(":BIT_DEP", OracleDbType.Decimal, BIT_DEP) ,                        
            //            ServiceUtils.CreateOracleParameter(":NOTE", OracleDbType.NVarchar2, null)

            //             });
            //        }
            //    }
            //    #endregion
            //    #region 固井参数
            //    if ((PRO_LOG_CEMENT1 != null && PRO_LOG_CEMENT1.Rows.Count > 0) || (PRO_LOG_CEMENT2 != null && PRO_LOG_CEMENT2.Rows.Count > 0) || (PRO_LOG_CEMENT3 != null && PRO_LOG_CEMENT3.Rows.Count > 0))
            //    {
            //        Double? CASING_SHOE_DEPTH = null;
            //        Double? CEMENT_PROPERTIES = null;
            //        Double? CEMENTED_QUANTITY = null;
            //        Double? DISTANCE_TUBING_AND_BUSHING = null;
            //        Double? CASING_TOP_SPACING = null;
            //        DateTime? CEMENT_WELL_DATE = null;
            //        DateTime? OPEN_WELL_DATE = null;
            //        if (PRO_LOG_CASIN != null && PRO_LOG_CASIN.Rows.Count > 0)
            //        {
            //            if (PRO_LOG_CASIN.Rows[0]["RUNNING_BOTTOM_DEPTH"] != null && PRO_LOG_CASIN.Rows[0]["RUNNING_BOTTOM_DEPTH"].ToString() != "") CASING_SHOE_DEPTH = Convert.ToDouble(PRO_LOG_CASIN.Rows[0]["RUNNING_BOTTOM_DEPTH"].ToString());
            //        }
            //        if (PRO_LOG_CEMENT1 != null && PRO_LOG_CEMENT1.Rows.Count > 0)
            //        {
            //            if (PRO_LOG_CEMENT1.Rows[0]["CEMENT_TYPE"] != null && PRO_LOG_CEMENT1.Rows[0]["CEMENT_TYPE"].ToString() != "") CEMENT_PROPERTIES = Convert.ToDouble(PRO_LOG_CEMENT1.Rows[0]["CEMENT_TYPE"].ToString());
            //            if (PRO_LOG_CEMENT1.Rows[0]["CEMENT_USAGE"] != null && PRO_LOG_CEMENT1.Rows[0]["CEMENT_USAGE"].ToString() != "") CEMENTED_QUANTITY = Convert.ToDouble(PRO_LOG_CEMENT1.Rows[0]["CEMENT_USAGE"].ToString());
            //        }
            //        if (PRO_LOG_CEMENT3 != null && PRO_LOG_CEMENT3.Rows.Count > 0)
            //        {
            //            if (PRO_LOG_CEMENT3.Rows[0]["DISTANCE_TUBING_BUSHING"] != null && PRO_LOG_CEMENT3.Rows[0]["DISTANCE_TUBING_BUSHING"].ToString() != "") DISTANCE_TUBING_AND_BUSHING = Convert.ToDouble(PRO_LOG_CEMENT3.Rows[0]["DISTANCE_TUBING_BUSHING"].ToString());
            //            if (PRO_LOG_CEMENT3.Rows[0]["DISTANCE_CASING_BUSHING"] != null && PRO_LOG_CEMENT3.Rows[0]["DISTANCE_CASING_BUSHING"].ToString() != "") CASING_TOP_SPACING = Convert.ToDouble(PRO_LOG_CEMENT3.Rows[0]["DISTANCE_CASING_BUSHING"].ToString());
            //        }
            //        if (PRO_LOG_CEMENT2 != null && PRO_LOG_CEMENT2.Rows.Count > 0)
            //        {
            //            if (PRO_LOG_CEMENT2.Rows[0]["CEMENT_DATE"] != null && PRO_LOG_CEMENT2.Rows[0]["CEMENT_DATE"].ToString() != "") CEMENT_WELL_DATE = Convert.ToDateTime(PRO_LOG_CEMENT2.Rows[0]["CEMENT_DATE"].ToString());
            //            if (PRO_LOG_CEMENT2.Rows[0]["CEMENT_END_DATE"] != null && PRO_LOG_CEMENT2.Rows[0]["CEMENT_END_DATE"].ToString() != "") OPEN_WELL_DATE = Convert.ToDateTime(PRO_LOG_CEMENT2.Rows[0]["CEMENT_END_DATE"].ToString());
            //        }
            //        strSql.Append("insert into PRO_LOG_CEMENT(");
            //        strSql.Append("CEMENTID,JOB_PLAN_CD,REQUISITION_CD,UPDATE_DATE,CEMENT_WELL_DATE,OPEN_WELL_DATE,DISTANCE_TUBING_AND_BUSHING,CASING_TOP_SPACING,CEMENT_PROPERTIES,CEMENTED_QUANTITY,CASING_SHOE_DEPTH)");
            //        strSql.Append(" values (");
            //        strSql.Append("CEMENTID_SEQ.nextval,:JOB_PLAN_CD,:REQUISITION_CD,:UPDATE_DATE,:CEMENT_WELL_DATE,:OPEN_WELL_DATE,:DISTANCE_TUBING_AND_BUSHING,:CASING_TOP_SPACING,:CEMENT_PROPERTIES,:CEMENTED_QUANTITY,:CASING_SHOE_DEPTH);");
            //        parameters.Add(ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, JOB_PLAN_CD));
            //        parameters.Add(ServiceUtils.CreateOracleParameter(":REQUISITION_CD", OracleDbType.NVarchar2, REQUISITION_CD));
            //        parameters.AddRange(new OracleParameter[] {          
            //            ServiceUtils.CreateOracleParameter(":UPDATE_DATE", OracleDbType.Date, DateTime.Now),
            //            ServiceUtils.CreateOracleParameter(":CEMENT_WELL_DATE", OracleDbType.Date, CEMENT_WELL_DATE),
            //            ServiceUtils.CreateOracleParameter(":OPEN_WELL_DATE", OracleDbType.Date, OPEN_WELL_DATE),
            //            ServiceUtils.CreateOracleParameter(":DISTANCE_TUBING_AND_BUSHING", OracleDbType.Decimal, DISTANCE_TUBING_AND_BUSHING),
            //            ServiceUtils.CreateOracleParameter(":CASING_TOP_SPACING", OracleDbType.Decimal, CASING_TOP_SPACING),
            //            ServiceUtils.CreateOracleParameter(":CEMENT_PROPERTIES", OracleDbType.Decimal, CEMENT_PROPERTIES),
            //            ServiceUtils.CreateOracleParameter(":CEMENTED_QUANTITY", OracleDbType.Decimal, CEMENTED_QUANTITY),
            //            ServiceUtils.CreateOracleParameter(":CASING_SHOE_DEPTH", OracleDbType.Decimal, CASING_SHOE_DEPTH)
            //        });
            //    }
            //    #endregion
            //    strSql.Append(" end;");
            //    if (DbHelperOra.ExecuteSql(strSql.ToString(), parameters.ToArray()) > 0) return true;
            //}
            #endregion
            return false;
        }
        /// <summary>
        /// 存储收集信息中取芯数据
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public bool Save_A12取芯数据(string REQUISITION_CD, string JOB_PLAN_CD, string WELL_JOB_NAME)
        {
            if (WELL_JOB_NAME == null) return false;
            var PRO_LOG_CORE = GetData_A12取心数据(WELL_JOB_NAME).Tables[0];

            if (PRO_LOG_CORE != null && PRO_LOG_CORE.Rows.Count > 0)
            {
                Double? START_DEPTH = null;
                Double? END_DEPTH = null;
                if (PRO_LOG_CORE.Rows[0]["TRIPIN_MD"] != null || PRO_LOG_CORE.Rows[0]["TRIPIN_MD"].ToString() != "")
                    START_DEPTH = Convert.ToDouble(PRO_LOG_CORE.Rows[0]["TRIPIN_MD"].ToString());
                if (PRO_LOG_CORE.Rows[0]["CORING_DEPTH"] != null || PRO_LOG_CORE.Rows[0]["CORING_DEPTH"].ToString() != "")
                    END_DEPTH = Convert.ToDouble(PRO_LOG_CORE.Rows[0]["CORING_DEPTH"].ToString());

                StringBuilder strSql = new StringBuilder();
                var parameters = new List<OracleParameter>();
                strSql.Append("Begin ");
                strSql.Append("insert into PRO_LOG_CORE(");
                strSql.Append("COREID,JOB_PLAN_CD,REQUISITION_CD,UPDATE_DATE,START_DEPTH,END_DEPTH");
                strSql.Append(") values (");
                strSql.Append("COREID_SEQ.nextval,:JOB_PLAN_CD,:REQUISITION_CD,:UPDATE_DATE,:START_DEPTH,:END_DEPTH");
                strSql.Append(") ;");
                //parameters.Add(ServiceUtils.CreateOracleParameter(":COREID", OracleDbType.Varchar2, GetCOREID()));
                parameters.Add(ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, JOB_PLAN_CD));
                parameters.Add(ServiceUtils.CreateOracleParameter(":REQUISITION_CD", OracleDbType.Varchar2, REQUISITION_CD));
                parameters.AddRange(new OracleParameter[] {          
                        ServiceUtils.CreateOracleParameter(":UPDATE_DATE", OracleDbType.Date, DateTime.Now) ,                 
                        ServiceUtils.CreateOracleParameter(":START_DEPTH", OracleDbType.Decimal, START_DEPTH) ,            
                        ServiceUtils.CreateOracleParameter(":END_DEPTH", OracleDbType.Decimal, END_DEPTH)
                         });
                strSql.Append(" end;");
                if (DbHelperOra.ExecuteSql(strSql.ToString(), parameters.ToArray()) > 0) return true;
            }
            return false;
        }
        /// <summary>
        /// 存储收集信息中钻井液全性能
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public bool Save_A12钻井液全性能(string REQUISITION_CD, string JOB_PLAN_CD, string WELL_JOB_NAME)
        {
            if (WELL_JOB_NAME == null) return false;
            var PRO_LOG_SLOP = GetData_A12钻井液全性能(WELL_JOB_NAME).Tables[0];

            if (PRO_LOG_SLOP != null && PRO_LOG_SLOP.Rows.Count > 0)
            {
                double? P_XN_MUD_DENSITY = null;
                double? SLOP_PH = null;
                double? DRILL_FLU_VISC = null;
                double? SLOP_TEMPERATURE = null;
                double? MUD_FILTRATE_SALINITY = null;
                if (PRO_LOG_SLOP.Rows[0]["FLUID_DENSITY"] != null && PRO_LOG_SLOP.Rows[0]["FLUID_DENSITY"].ToString() != "")
                    P_XN_MUD_DENSITY = Convert.ToDouble(PRO_LOG_SLOP.Rows[0]["FLUID_DENSITY"].ToString());
                if (PRO_LOG_SLOP.Rows[0]["PH"] != null && PRO_LOG_SLOP.Rows[0]["PH"].ToString() != "")
                    SLOP_PH = Convert.ToDouble(PRO_LOG_SLOP.Rows[0]["PH"].ToString());
                if (PRO_LOG_SLOP.Rows[0]["PV"] != null && PRO_LOG_SLOP.Rows[0]["PV"].ToString() != "")
                    DRILL_FLU_VISC = Convert.ToDouble(PRO_LOG_SLOP.Rows[0]["PV"].ToString());
                if (PRO_LOG_SLOP.Rows[0]["MTO"] != null && PRO_LOG_SLOP.Rows[0]["MTO"].ToString() != "")
                    SLOP_TEMPERATURE = Convert.ToDouble(PRO_LOG_SLOP.Rows[0]["MTO"].ToString());
                if (PRO_LOG_SLOP.Rows[0]["TOTAL_FILTRATE_SALINITY"] != null && PRO_LOG_SLOP.Rows[0]["TOTAL_FILTRATE_SALINITY"].ToString() != "")
                    MUD_FILTRATE_SALINITY = Convert.ToDouble(PRO_LOG_SLOP.Rows[0]["TOTAL_FILTRATE_SALINITY"].ToString());


                StringBuilder strSql = new StringBuilder();
                var parameters = new List<OracleParameter>();
                strSql.Append("Begin ");
                strSql.Append("insert into PRO_LOG_SLOP(");
                strSql.Append("MUDID,JOB_PLAN_CD,REQUISITION_CD,UPDATE_DATE,JTLT,SLOP_PROPERTIES,P_XN_MUD_DENSITY,SLOP_PERSENT,SLOP_PH,DRILL_FLU_VISC,SLOP_TEMPERATURE,MUD_FILTRATE_SALINITY)");
                strSql.Append(" values (");
                strSql.Append("MUDID_SEQ.nextval,:JOB_PLAN_CD,:REQUISITION_CD,:UPDATE_DATE,:JTLT,:SLOP_PROPERTIES,:P_XN_MUD_DENSITY,:SLOP_PERSENT,:SLOP_PH,:DRILL_FLU_VISC,:SLOP_TEMPERATURE,:MUD_FILTRATE_SALINITY) ;");
                parameters.Add(ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, JOB_PLAN_CD));
                parameters.Add(ServiceUtils.CreateOracleParameter(":REQUISITION_CD", OracleDbType.NVarchar2, REQUISITION_CD));
                parameters.AddRange(new OracleParameter[] {          
                        ServiceUtils.CreateOracleParameter(":UPDATE_DATE", OracleDbType.Date,DateTime.Now) ,            
                        ServiceUtils.CreateOracleParameter(":JTLT", OracleDbType.Varchar2, PRO_LOG_SLOP.Rows[0]["FLUID_SYSTEM"].ToString()) ,            
                        ServiceUtils.CreateOracleParameter(":SLOP_PROPERTIES", OracleDbType.Varchar2, PRO_LOG_SLOP.Rows[0]["FLUID_SYSTEM"].ToString()) ,            
                        ServiceUtils.CreateOracleParameter(":P_XN_MUD_DENSITY", OracleDbType.Decimal, P_XN_MUD_DENSITY) ,            
                        ServiceUtils.CreateOracleParameter(":SLOP_PERSENT", OracleDbType.Decimal, P_XN_MUD_DENSITY),
                        ServiceUtils.CreateOracleParameter(":SLOP_PH", OracleDbType.Decimal, SLOP_PH) ,   
                        ServiceUtils.CreateOracleParameter(":DRILL_FLU_VISC", OracleDbType.Decimal, DRILL_FLU_VISC) ,            
                        ServiceUtils.CreateOracleParameter(":SLOP_TEMPERATURE", OracleDbType.Decimal, SLOP_TEMPERATURE) ,                      
                        ServiceUtils.CreateOracleParameter(":MUD_FILTRATE_SALINITY", OracleDbType.Decimal, MUD_FILTRATE_SALINITY)
                         });
                strSql.Append(" end;");
                if (DbHelperOra.ExecuteSql(strSql.ToString(), parameters.ToArray()) > 0) return true;
            }
            return false;
        }
        /// <summary>
        /// 存储收集信息中套管参数
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public bool Save_A12套管参数(string REQUISITION_CD, string JOB_PLAN_CD, string WELL_JOB_NAME)
        {
            if (WELL_JOB_NAME == null) return false;
            var PRO_LOG_CASIN = GetData_A12下套管记录(WELL_JOB_NAME).Tables[0];

            if (PRO_LOG_CASIN != null && PRO_LOG_CASIN.Rows.Count > 0)
            {
                Double? CASING_OUTSIZE = null;
                Double? CASING_QUANTITY = null;
                Double? PIPE_THICKNESS = null;
                Double? P_XN_CASING_LENGTH = null;
                Double? ACCUMULATIVE_LENGTH = null;
                Double? RUNNING_BOTTOM_DEPTH = null;
                Double? SCREW_ON_TORQUE = null;
                Double? CENTERING_DEVICE_POSITION = null;
                Double? CENTERING_DEVICE_NO = null;
                Double? CENTERING_DEVICE_SIZE = null;
                Double? BASETUBE_BORESIZE = null;
                Double? BASETUBE_OUTSIZE = null;
                Double? BASETUBE_UNITNUMBER = null;
                Double? BASETUBE_SLOTWIDTH = null;
                Double? BASETUBE_POREDIAMETER = null;
                Double? SIEVETUBE_SLOTWIDTH = null;
                Double? SIEVETUBE_SLOTLENGTH = null;
                Double? PIPE_HEIGHT = null;

                #region
                if (PRO_LOG_CASIN.Rows[0]["CASING_OUTER_DIAMETER"] != null && PRO_LOG_CASIN.Rows[0]["CASING_OUTER_DIAMETER"].ToString() != "")
                    CASING_OUTSIZE = Convert.ToDouble(PRO_LOG_CASIN.Rows[0]["CASING_OUTER_DIAMETER"].ToString());
                if (PRO_LOG_CASIN.Rows[0]["CASING_QUANTITY"] != null && PRO_LOG_CASIN.Rows[0]["CASING_QUANTITY"].ToString() != "")
                    CASING_QUANTITY = Convert.ToDouble(PRO_LOG_CASIN.Rows[0]["CASING_QUANTITY"].ToString());
                if (PRO_LOG_CASIN.Rows[0]["CASING_THICKNESS"] != null && PRO_LOG_CASIN.Rows[0]["CASING_THICKNESS"].ToString() != "")
                    PIPE_THICKNESS = Convert.ToDouble(PRO_LOG_CASIN.Rows[0]["CASING_THICKNESS"].ToString());
                if (PRO_LOG_CASIN.Rows[0]["CASING_LENGTH"] != null && PRO_LOG_CASIN.Rows[0]["CASING_LENGTH"].ToString() != "")
                    P_XN_CASING_LENGTH = Convert.ToDouble(PRO_LOG_CASIN.Rows[0]["CASING_LENGTH"].ToString());
                if (PRO_LOG_CASIN.Rows[0]["ACCUMULATIVE_LENGTH"] != null && PRO_LOG_CASIN.Rows[0]["ACCUMULATIVE_LENGTH"].ToString() != "")
                    ACCUMULATIVE_LENGTH = Convert.ToDouble(PRO_LOG_CASIN.Rows[0]["ACCUMULATIVE_LENGTH"].ToString());
                if (PRO_LOG_CASIN.Rows[0]["RUNNING_BOTTOM_DEPTH"] != null && PRO_LOG_CASIN.Rows[0]["RUNNING_BOTTOM_DEPTH"].ToString() != "")
                    RUNNING_BOTTOM_DEPTH = Convert.ToDouble(PRO_LOG_CASIN.Rows[0]["RUNNING_BOTTOM_DEPTH"].ToString());
                if (PRO_LOG_CASIN.Rows[0]["SCREW_ON_TORQUE"] != null && PRO_LOG_CASIN.Rows[0]["SCREW_ON_TORQUE"].ToString() != "")
                    SCREW_ON_TORQUE = Convert.ToDouble(PRO_LOG_CASIN.Rows[0]["SCREW_ON_TORQUE"].ToString());
                if (PRO_LOG_CASIN.Rows[0]["CENTERING_DEVICE_POSITION"] != null && PRO_LOG_CASIN.Rows[0]["CENTERING_DEVICE_POSITION"].ToString() != "")
                    CENTERING_DEVICE_POSITION = Convert.ToDouble(PRO_LOG_CASIN.Rows[0]["CENTERING_DEVICE_POSITION"].ToString());
                if (PRO_LOG_CASIN.Rows[0]["CENTERING_DEVICE_NO"] != null && PRO_LOG_CASIN.Rows[0]["CENTERING_DEVICE_NO"].ToString() != "")
                    CENTERING_DEVICE_NO = Convert.ToDouble(PRO_LOG_CASIN.Rows[0]["CENTERING_DEVICE_NO"].ToString());
                if (PRO_LOG_CASIN.Rows[0]["CENTERING_DEVICE_SIZE"] != null && PRO_LOG_CASIN.Rows[0]["CENTERING_DEVICE_SIZE"].ToString() != "")
                    CENTERING_DEVICE_SIZE = Convert.ToDouble(PRO_LOG_CASIN.Rows[0]["CENTERING_DEVICE_SIZE"].ToString());
                if (PRO_LOG_CASIN.Rows[0]["BASETUBE_BORESIZE"] != null && PRO_LOG_CASIN.Rows[0]["BASETUBE_BORESIZE"].ToString() != "")
                    BASETUBE_BORESIZE = Convert.ToDouble(PRO_LOG_CASIN.Rows[0]["BASETUBE_BORESIZE"].ToString());
                if (PRO_LOG_CASIN.Rows[0]["BASETUBE_OUTSIZE"] != null && PRO_LOG_CASIN.Rows[0]["BASETUBE_OUTSIZE"].ToString() != "")
                    BASETUBE_OUTSIZE = Convert.ToDouble(PRO_LOG_CASIN.Rows[0]["BASETUBE_OUTSIZE"].ToString());
                if (PRO_LOG_CASIN.Rows[0]["BASETUBE_UNITNUMBER"] != null && PRO_LOG_CASIN.Rows[0]["BASETUBE_UNITNUMBER"].ToString() != "")
                    BASETUBE_UNITNUMBER = Convert.ToDouble(PRO_LOG_CASIN.Rows[0]["BASETUBE_UNITNUMBER"].ToString());
                if (PRO_LOG_CASIN.Rows[0]["BASETUBE_SLOTWIDTH"] != null && PRO_LOG_CASIN.Rows[0]["BASETUBE_SLOTWIDTH"].ToString() != "")
                    BASETUBE_SLOTWIDTH = Convert.ToDouble(PRO_LOG_CASIN.Rows[0]["BASETUBE_SLOTWIDTH"].ToString());
                if (PRO_LOG_CASIN.Rows[0]["BASETUBE_POREDIAMETER"] != null && PRO_LOG_CASIN.Rows[0]["BASETUBE_POREDIAMETER"].ToString() != "")
                    BASETUBE_POREDIAMETER = Convert.ToDouble(PRO_LOG_CASIN.Rows[0]["BASETUBE_POREDIAMETER"].ToString());
                if (PRO_LOG_CASIN.Rows[0]["SIEVETUBE_SLOTWIDTH"] != null && PRO_LOG_CASIN.Rows[0]["SIEVETUBE_SLOTWIDTH"].ToString() != "")
                    SIEVETUBE_SLOTWIDTH = Convert.ToDouble(PRO_LOG_CASIN.Rows[0]["SIEVETUBE_SLOTWIDTH"].ToString());
                if (PRO_LOG_CASIN.Rows[0]["SIEVETUBE_SLOTLENGTH"] != null && PRO_LOG_CASIN.Rows[0]["SIEVETUBE_SLOTLENGTH"].ToString() != "")
                    SIEVETUBE_SLOTLENGTH = Convert.ToDouble(PRO_LOG_CASIN.Rows[0]["SIEVETUBE_SLOTLENGTH"].ToString());
                if (PRO_LOG_CASIN.Rows[0]["RUNNING_BOTTOM_DEPTH"] != null && PRO_LOG_CASIN.Rows[0]["RUNNING_BOTTOM_DEPTH"].ToString() != "")
                    PIPE_HEIGHT = Convert.ToDouble(PRO_LOG_CASIN.Rows[0]["RUNNING_BOTTOM_DEPTH"].ToString());
                #endregion
                StringBuilder strSql = new StringBuilder();
                var parameters = new List<OracleParameter>();
                strSql.Append("Begin ");
                strSql.Append("insert into PRO_LOG_CASIN(");
                strSql.Append("CASINID,JOB_PLAN_CD,REQUISITION_CD,UPDATE_DATE,CASING_NAME,CASING_TYPE,CASING_OUTSIZE,CASING_QUANTITY,CASING_THREAD_TYPE,CASING_GRADE,PIPE_THICKNESS,P_XN_CASING_LENGTH,ACCUMULATIVE_LENGTH,RUNNING_BOTTOM_DEPTH,SCREW_ON_TORQUE,CENTERING_DEVICE_POSITION,CENTERING_DEVICE_NO,CENTERING_DEVICE_TYPE,CENTERING_DEVICE_SIZE,BASETUBE_BORESIZE,BASETUBE_OUTSIZE,BASETUBE_UNITNUMBER,BASETUBE_TYPE,BASETUBE_SLOTWIDTH,BASETUBE_POREDIAMETER,SIEVETUBE_SLOTWIDTH,SIEVETUBE_SLOTLENGTH,PIPE_HEIGHT");
                strSql.Append(") values (");
                strSql.Append("CASINID_SEQ.nextval,:JOB_PLAN_CD,:REQUISITION_CD,:UPDATE_DATE,:CASING_NAME,:CASING_TYPE,:CASING_OUTSIZE,:CASING_QUANTITY,:CASING_THREAD_TYPE,:CASING_GRADE,:PIPE_THICKNESS,:P_XN_CASING_LENGTH,:ACCUMULATIVE_LENGTH,:RUNNING_BOTTOM_DEPTH,:SCREW_ON_TORQUE,:CENTERING_DEVICE_POSITION,:CENTERING_DEVICE_NO,:CENTERING_DEVICE_TYPE,:CENTERING_DEVICE_SIZE,:BASETUBE_BORESIZE,:BASETUBE_OUTSIZE,:BASETUBE_UNITNUMBER,:BASETUBE_TYPE,:BASETUBE_SLOTWIDTH,:BASETUBE_POREDIAMETER,:SIEVETUBE_SLOTWIDTH,:SIEVETUBE_SLOTLENGTH,:PIPE_HEIGHT");
                strSql.Append(") ;");
                parameters.Add(ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, JOB_PLAN_CD));
                parameters.Add(ServiceUtils.CreateOracleParameter(":REQUISITION_CD", OracleDbType.NVarchar2, REQUISITION_CD));
                parameters.AddRange(new OracleParameter[] {          
                        ServiceUtils.CreateOracleParameter(":UPDATE_DATE", OracleDbType.Date,DateTime.Now) ,             
                        ServiceUtils.CreateOracleParameter(":CASING_NAME", OracleDbType.Varchar2, PRO_LOG_CASIN.Rows[0]["CASING_NAME"].ToString()) ,            
                        ServiceUtils.CreateOracleParameter(":CASING_TYPE", OracleDbType.Varchar2, PRO_LOG_CASIN.Rows[0]["BASETUBE_TYPE"].ToString()) ,
                        ServiceUtils.CreateOracleParameter(":CASING_OUTSIZE", OracleDbType.Decimal, CASING_OUTSIZE),
                        ServiceUtils.CreateOracleParameter(":CASING_QUANTITY", OracleDbType.Decimal, CASING_QUANTITY) , 
                        ServiceUtils.CreateOracleParameter(":CASING_THREAD_TYPE", OracleDbType.Varchar2,PRO_LOG_CASIN.Rows[0]["CASING_THREAD_TYPE"].ToString()) ,            
                        ServiceUtils.CreateOracleParameter(":CASING_GRADE", OracleDbType.Varchar2, PRO_LOG_CASIN.Rows[0]["STEEL_GRADE"].ToString()) ,            
                        ServiceUtils.CreateOracleParameter(":PIPE_THICKNESS", OracleDbType.Decimal, PIPE_THICKNESS) ,    
                         ServiceUtils.CreateOracleParameter(":P_XN_CASING_LENGTH", OracleDbType.Decimal, P_XN_CASING_LENGTH) ,            
                        ServiceUtils.CreateOracleParameter(":ACCUMULATIVE_LENGTH", OracleDbType.Decimal, ACCUMULATIVE_LENGTH) ,            
                        ServiceUtils.CreateOracleParameter(":RUNNING_BOTTOM_DEPTH", OracleDbType.Decimal, RUNNING_BOTTOM_DEPTH) ,    
                         ServiceUtils.CreateOracleParameter(":SCREW_ON_TORQUE", OracleDbType.Decimal, SCREW_ON_TORQUE) ,            
                        ServiceUtils.CreateOracleParameter(":CENTERING_DEVICE_POSITION", OracleDbType.Decimal, CENTERING_DEVICE_POSITION) ,            
                        ServiceUtils.CreateOracleParameter(":CENTERING_DEVICE_NO", OracleDbType.Decimal, CENTERING_DEVICE_NO) ,    
                         ServiceUtils.CreateOracleParameter(":CENTERING_DEVICE_TYPE", OracleDbType.Varchar2, PRO_LOG_CASIN.Rows[0]["CENTERING_DEVICE_TYPE"].ToString()) ,            
                        ServiceUtils.CreateOracleParameter(":CENTERING_DEVICE_SIZE", OracleDbType.Decimal, CENTERING_DEVICE_SIZE) ,            
                        ServiceUtils.CreateOracleParameter(":BASETUBE_BORESIZE", OracleDbType.Decimal, BASETUBE_BORESIZE) ,    
                        ServiceUtils.CreateOracleParameter(":BASETUBE_OUTSIZE", OracleDbType.Decimal, BASETUBE_OUTSIZE) ,            
                        ServiceUtils.CreateOracleParameter(":BASETUBE_UNITNUMBER", OracleDbType.Decimal, BASETUBE_UNITNUMBER) ,            
                        ServiceUtils.CreateOracleParameter(":BASETUBE_TYPE", OracleDbType.Varchar2, PRO_LOG_CASIN.Rows[0]["BASETUBE_TYPE"].ToString()) ,    
                        ServiceUtils.CreateOracleParameter(":BASETUBE_SLOTWIDTH", OracleDbType.Decimal, BASETUBE_SLOTWIDTH) ,            
                        ServiceUtils.CreateOracleParameter(":BASETUBE_POREDIAMETER", OracleDbType.Decimal, BASETUBE_POREDIAMETER) ,            
                        ServiceUtils.CreateOracleParameter(":SIEVETUBE_SLOTWIDTH", OracleDbType.Decimal, SIEVETUBE_SLOTWIDTH) ,    
                        ServiceUtils.CreateOracleParameter(":SIEVETUBE_SLOTLENGTH", OracleDbType.Decimal, SIEVETUBE_SLOTLENGTH) ,            
                        ServiceUtils.CreateOracleParameter(":PIPE_HEIGHT", OracleDbType.Decimal, PIPE_HEIGHT)    
            });
                strSql.Append(" end;");
                if (DbHelperOra.ExecuteSql(strSql.ToString(), parameters.ToArray()) > 0) return true;
            }
            return false;
        }
        /// <summary>
        /// 存储收集信息中钻头使用情况信息
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public bool Save_A12钻头使用情况信息(string REQUISITION_CD, string JOB_PLAN_CD, string WELL_JOB_NAME)
        {
            if (WELL_JOB_NAME == null) return false;
            var PRO_LOG_BIT_PROGRAM = GetData_A12钻头使用情况信息(WELL_JOB_NAME).Tables[0];

            if (PRO_LOG_BIT_PROGRAM != null && PRO_LOG_BIT_PROGRAM.Rows.Count > 0)
            {
                Double? BIT_SIZE = null;
                Double? BIT_DEP = null;
                if (PRO_LOG_BIT_PROGRAM.Rows[0]["BIT_SIZE"] != null && PRO_LOG_BIT_PROGRAM.Rows[0]["BIT_SIZE"].ToString() != "")
                    BIT_SIZE = Convert.ToDouble(PRO_LOG_BIT_PROGRAM.Rows[0]["BIT_SIZE"].ToString());
                if (PRO_LOG_BIT_PROGRAM.Rows[0]["TRIPOUT_MD"] != null && PRO_LOG_BIT_PROGRAM.Rows[0]["TRIPOUT_MD"].ToString() != "")
                    BIT_DEP = Convert.ToDouble(PRO_LOG_BIT_PROGRAM.Rows[0]["TRIPOUT_MD"].ToString());

                StringBuilder strSql = new StringBuilder();
                var parameters = new List<OracleParameter>();
                strSql.Append("Begin ");
                strSql.Append("insert into PRO_LOG_BIT_PROGRAM(");
                strSql.Append("BITID,JOB_PLAN_CD,REQUISITION_CD,UPDATE_DATE,BIT_SIZE,BIT_DEP,NOTE)");
                strSql.Append(" values (");
                strSql.Append("BITID_SEQ.nextval,:JOB_PLAN_CD,:REQUISITION_CD,:UPDATE_DATE,:BIT_SIZE,:BIT_DEP,:NOTE) ;");
                parameters.Add(ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, JOB_PLAN_CD));
                parameters.Add(ServiceUtils.CreateOracleParameter(":REQUISITION_CD", OracleDbType.NVarchar2, REQUISITION_CD));
                parameters.AddRange(new OracleParameter[] {          
                        ServiceUtils.CreateOracleParameter(":UPDATE_DATE", OracleDbType.Date, DateTime.Now) ,            
                        ServiceUtils.CreateOracleParameter(":BIT_SIZE", OracleDbType.Decimal, BIT_SIZE) ,            
                        ServiceUtils.CreateOracleParameter(":BIT_DEP", OracleDbType.Decimal, BIT_DEP) ,                        
                        ServiceUtils.CreateOracleParameter(":NOTE", OracleDbType.NVarchar2, null)

                         });
                strSql.Append(" end;");
                if (DbHelperOra.ExecuteSql(strSql.ToString(), parameters.ToArray()) > 0) return true;
            }
            return false;
        }
        /// <summary>
        /// 存储收集信息中固井参数
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public bool Save_A12固井参数(string REQUISITION_CD, string JOB_PLAN_CD, string WELL_JOB_NAME)
        {
            if (WELL_JOB_NAME == null) return false;
            var PRO_LOG_CASIN = GetData_A12下套管记录(WELL_JOB_NAME).Tables[0];
            var PRO_LOG_CEMENT1 = GetData_A12水泥浆信息(WELL_JOB_NAME).Tables[0];
            var PRO_LOG_CEMENT2 = GetData_A12固井基础数据(WELL_JOB_NAME).Tables[0];
            var PRO_LOG_CEMENT3 = GetData_A12钻井施工基础数据(WELL_JOB_NAME).Tables[0];

            if ((PRO_LOG_CEMENT1 != null && PRO_LOG_CEMENT1.Rows.Count > 0) || (PRO_LOG_CEMENT2 != null && PRO_LOG_CEMENT2.Rows.Count > 0) || (PRO_LOG_CEMENT3 != null && PRO_LOG_CEMENT3.Rows.Count > 0))
            {
                Double? CASING_SHOE_DEPTH = null;
                Double? CEMENT_PROPERTIES = null;
                Double? CEMENTED_QUANTITY = null;
                Double? DISTANCE_TUBING_AND_BUSHING = null;
                Double? CASING_TOP_SPACING = null;
                DateTime? CEMENT_WELL_DATE = null;
                DateTime? OPEN_WELL_DATE = null;
                if (PRO_LOG_CASIN != null && PRO_LOG_CASIN.Rows.Count > 0)
                {
                    if (PRO_LOG_CASIN.Rows[0]["RUNNING_BOTTOM_DEPTH"] != null && PRO_LOG_CASIN.Rows[0]["RUNNING_BOTTOM_DEPTH"].ToString() != "")
                        CASING_SHOE_DEPTH = Convert.ToDouble(PRO_LOG_CASIN.Rows[0]["RUNNING_BOTTOM_DEPTH"].ToString());
                }
                if (PRO_LOG_CEMENT1 != null && PRO_LOG_CEMENT1.Rows.Count > 0)
                {
                    if (PRO_LOG_CEMENT1.Rows[0]["CEMENT_TYPE"] != null && PRO_LOG_CEMENT1.Rows[0]["CEMENT_TYPE"].ToString() != "")
                        CEMENT_PROPERTIES = Convert.ToDouble(PRO_LOG_CEMENT1.Rows[0]["CEMENT_TYPE"].ToString());
                    if (PRO_LOG_CEMENT1.Rows[0]["CEMENT_USAGE"] != null && PRO_LOG_CEMENT1.Rows[0]["CEMENT_USAGE"].ToString() != "")
                        CEMENTED_QUANTITY = Convert.ToDouble(PRO_LOG_CEMENT1.Rows[0]["CEMENT_USAGE"].ToString());
                }
                if (PRO_LOG_CEMENT3 != null && PRO_LOG_CEMENT3.Rows.Count > 0)
                {
                    if (PRO_LOG_CEMENT3.Rows[0]["DISTANCE_TUBING_BUSHING"] != null && PRO_LOG_CEMENT3.Rows[0]["DISTANCE_TUBING_BUSHING"].ToString() != "")
                        DISTANCE_TUBING_AND_BUSHING = Convert.ToDouble(PRO_LOG_CEMENT3.Rows[0]["DISTANCE_TUBING_BUSHING"].ToString());
                    if (PRO_LOG_CEMENT3.Rows[0]["DISTANCE_CASING_BUSHING"] != null && PRO_LOG_CEMENT3.Rows[0]["DISTANCE_CASING_BUSHING"].ToString() != "")
                        CASING_TOP_SPACING = Convert.ToDouble(PRO_LOG_CEMENT3.Rows[0]["DISTANCE_CASING_BUSHING"].ToString());
                }
                if (PRO_LOG_CEMENT2 != null && PRO_LOG_CEMENT2.Rows.Count > 0)
                {
                    if (PRO_LOG_CEMENT2.Rows[0]["CEMENT_DATE"] != null && PRO_LOG_CEMENT2.Rows[0]["CEMENT_DATE"].ToString() != "")
                        CEMENT_WELL_DATE = Convert.ToDateTime(PRO_LOG_CEMENT2.Rows[0]["CEMENT_DATE"].ToString());
                    if (PRO_LOG_CEMENT2.Rows[0]["CEMENT_END_DATE"] != null && PRO_LOG_CEMENT2.Rows[0]["CEMENT_END_DATE"].ToString() != "")
                        OPEN_WELL_DATE = Convert.ToDateTime(PRO_LOG_CEMENT2.Rows[0]["CEMENT_END_DATE"].ToString());
                }
                StringBuilder strSql = new StringBuilder();
                var parameters = new List<OracleParameter>();
                strSql.Append("Begin ");
                strSql.Append("insert into PRO_LOG_CEMENT(");
                strSql.Append("CEMENTID,JOB_PLAN_CD,REQUISITION_CD,UPDATE_DATE,CEMENT_WELL_DATE,OPEN_WELL_DATE,DISTANCE_TUBING_AND_BUSHING,CASING_TOP_SPACING,CEMENT_PROPERTIES,CEMENTED_QUANTITY,CASING_SHOE_DEPTH)");
                strSql.Append(" values (");
                strSql.Append("CEMENTID_SEQ.nextval,:JOB_PLAN_CD,:REQUISITION_CD,:UPDATE_DATE,:CEMENT_WELL_DATE,:OPEN_WELL_DATE,:DISTANCE_TUBING_AND_BUSHING,:CASING_TOP_SPACING,:CEMENT_PROPERTIES,:CEMENTED_QUANTITY,:CASING_SHOE_DEPTH);");
                parameters.Add(ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, JOB_PLAN_CD));
                parameters.Add(ServiceUtils.CreateOracleParameter(":REQUISITION_CD", OracleDbType.NVarchar2, REQUISITION_CD));
                parameters.AddRange(new OracleParameter[] {          
                        ServiceUtils.CreateOracleParameter(":UPDATE_DATE", OracleDbType.Date, DateTime.Now),
                        ServiceUtils.CreateOracleParameter(":CEMENT_WELL_DATE", OracleDbType.Date, CEMENT_WELL_DATE),
                        ServiceUtils.CreateOracleParameter(":OPEN_WELL_DATE", OracleDbType.Date, OPEN_WELL_DATE),
                        ServiceUtils.CreateOracleParameter(":DISTANCE_TUBING_AND_BUSHING", OracleDbType.Decimal, DISTANCE_TUBING_AND_BUSHING),
                        ServiceUtils.CreateOracleParameter(":CASING_TOP_SPACING", OracleDbType.Decimal, CASING_TOP_SPACING),
                        ServiceUtils.CreateOracleParameter(":CEMENT_PROPERTIES", OracleDbType.Decimal, CEMENT_PROPERTIES),
                        ServiceUtils.CreateOracleParameter(":CEMENTED_QUANTITY", OracleDbType.Decimal, CEMENTED_QUANTITY),
                        ServiceUtils.CreateOracleParameter(":CASING_SHOE_DEPTH", OracleDbType.Decimal, CASING_SHOE_DEPTH)
                    });
                strSql.Append(" end;");
                if (DbHelperOra.ExecuteSql(strSql.ToString(), parameters.ToArray()) > 0) return true;
            }
            return false;
        }
        #endregion

        #region 施工总结会DM_LOG_JOB_SUMMAY
        //public string GetJOB_SUMMARY_ID()
        //{
        //    var num = DbHelperOra.GetSingle(" select  max(to_number(JOB_SUMMARY_ID))+1  From  DM_LOG_JOB_SUMMAY ");
        //    return num == null ? "1" : num.ToString();
        //}
        [WebMethod(EnableSession = true)]
        public bool Save_施工总结会(byte[] data_施工总结)
        {
            if (data_施工总结 == null) return false;
            Model.DM_LOG_JOB_SUMMAY model = Utility.ModelHelper.DeserializeObject(data_施工总结) as Model.DM_LOG_JOB_SUMMAY;
            if (model == null) return false;
            model.SUMMARY_ITEM6 = null;
            model.SUMMARY_ITEM4 = null;
            model.SUMMARY_ITEM5 = null;
            model.NOTE = null;

            Workflow.Controller.ValidateSave<Workflow.C测井现场提交信息>(model.JOB_PLAN_CD);

            StringBuilder strSql = new StringBuilder();
            List<OracleParameter> parameters = new List<OracleParameter>();
            if (string.IsNullOrWhiteSpace(model.JOB_SUMMARY_ID))
            {
                strSql.Append("insert into DM_LOG_JOB_SUMMAY (");
                strSql.Append("JOB_SUMMARY_ID,JOB_PLAN_CD,MEETING_DATE,MEETING_TIME,HOST,WRITER,PARTICIPANTS,SUMMARY_ITEM1,SUMMARY_ITEM2,SUMMARY_ITEM3,SUMMARY_ITEM6,SUMMARY_ITEM4,SUMMARY_ITEM5,NOTE,ATTACH_INFO,ATTACH_FILE)");
                strSql.Append(" values (");
                strSql.Append("JOB_SUMMARY_ID_SEQ.nextval,:JOB_PLAN_CD,:MEETING_DATE,:MEETING_TIME,:HOST,:WRITER,:PARTICIPANTS,:SUMMARY_ITEM1,:SUMMARY_ITEM2,:SUMMARY_ITEM3,:SUMMARY_ITEM6,:SUMMARY_ITEM4,:SUMMARY_ITEM5,:NOTE,:ATTACH_INFO,:ATTACH_FILE) ");
                //parameters.Add(ServiceUtils.CreateOracleParameter(":JOB_SUMMARY_ID", OracleDbType.Varchar2, GetJOB_SUMMARY_ID()));
                parameters.Add(ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, model.JOB_PLAN_CD));
            }
            else
            {
                strSql.Append("update DM_LOG_JOB_SUMMAY set ");
                //strSql.Append("JOB_PLAN_CD=:JOB_PLAN_CD,");
                strSql.Append("MEETING_DATE=:MEETING_DATE,");
                strSql.Append("MEETING_TIME=:MEETING_TIME,");
                strSql.Append("HOST=:HOST,");
                strSql.Append("WRITER=:WRITER,");
                strSql.Append("PARTICIPANTS=:PARTICIPANTS,");
                strSql.Append("SUMMARY_ITEM1=:SUMMARY_ITEM1,");
                strSql.Append("SUMMARY_ITEM2=:SUMMARY_ITEM2,");
                strSql.Append("SUMMARY_ITEM3=:SUMMARY_ITEM3,");
                strSql.Append("SUMMARY_ITEM6=:SUMMARY_ITEM6,");
                strSql.Append("SUMMARY_ITEM4=:SUMMARY_ITEM4,");
                strSql.Append("SUMMARY_ITEM5=:SUMMARY_ITEM5,");
                strSql.Append("NOTE=:NOTE,");
                strSql.Append("ATTACH_INFO=:ATTACH_INFO,");
                strSql.Append("ATTACH_FILE=:ATTACH_FILE");
                strSql.Append(" where JOB_SUMMARY_ID=:JOB_SUMMARY_ID ");
                parameters.Add(ServiceUtils.CreateOracleParameter(":JOB_SUMMARY_ID", OracleDbType.Char, model.JOB_SUMMARY_ID));
            }
            parameters.AddRange(new OracleParameter[] {          
                        ServiceUtils.CreateOracleParameter(":MEETING_DATE", OracleDbType.Date, model.MEETING_DATE) ,            
                        ServiceUtils.CreateOracleParameter(":MEETING_TIME", OracleDbType.Decimal, model.MEETING_TIME) ,            
                        ServiceUtils.CreateOracleParameter(":HOST", OracleDbType.Char, model.HOST) ,            
                        ServiceUtils.CreateOracleParameter(":WRITER", OracleDbType.Char, model.WRITER) ,            
                        ServiceUtils.CreateOracleParameter(":PARTICIPANTS", OracleDbType.Char, model.PARTICIPANTS) ,            
                        ServiceUtils.CreateOracleParameter(":SUMMARY_ITEM1", OracleDbType.Char, model.SUMMARY_ITEM1) ,            
                        ServiceUtils.CreateOracleParameter(":SUMMARY_ITEM2", OracleDbType.Char, model.SUMMARY_ITEM2) ,            
                        ServiceUtils.CreateOracleParameter(":SUMMARY_ITEM3", OracleDbType.Char, model.SUMMARY_ITEM3) ,   
                        ServiceUtils.CreateOracleParameter(":SUMMARY_ITEM6", OracleDbType.Char, model.SUMMARY_ITEM6) ,            
                        ServiceUtils.CreateOracleParameter(":SUMMARY_ITEM4", OracleDbType.Char, model.SUMMARY_ITEM4) ,            
                        ServiceUtils.CreateOracleParameter(":SUMMARY_ITEM5", OracleDbType.Char, model.SUMMARY_ITEM5) ,
                        ServiceUtils.CreateOracleParameter(":NOTE", OracleDbType.Varchar2, model.NOTE),
                        ServiceUtils.CreateOracleParameter(":ATTACH_INFO", OracleDbType.Varchar2, model.ATTACH_INFO),
                        ServiceUtils.CreateOracleParameter(":ATTACH_FILE", OracleDbType.Blob, model.ATTACH_FILE)
            });

            if (DbHelperOra.ExecuteSqlTran(strSql.ToString(), parameters.ToArray()) > 0) return true;
            return false;
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetData_施工总结会(string string_作业计划书编号)
        {
            if (string.IsNullOrWhiteSpace(string_作业计划书编号)) return null;
            return DbHelperOra.Query("select * from DM_LOG_JOB_SUMMAY where job_plan_cd=:JOB_PLAN_CD",
                new OracleParameter[] { ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, string_作业计划书编号) });
        }
        #endregion
        #region 井场班前会DM_LOG_CLASS_MEET
        ///// <summary>
        ///// 获取井场班前会ID
        ///// </summary>
        ///// <returns></returns>
        //[WebMethod(EnableSession = true)]
        //public string GetCLASS_MEET_ID()
        //{
        //    var num = DbHelperOra.GetSingle(" select  max(to_number(CLASS_MEET_ID))+1  From  DM_LOG_CLASS_MEET ");
        //    return num == null ? "1" : num.ToString();
        //}
        [WebMethod(EnableSession = true)]
        public bool Save_井场班前会(byte[] data_井场班前会)
        {
            if (data_井场班前会 == null) return false;
            Model.DM_LOG_CLASS_MEET model = Utility.ModelHelper.DeserializeObject(data_井场班前会) as Model.DM_LOG_CLASS_MEET;
            if (model == null) return false;
            model.SUMMARY_ITEM6 = null;
            model.SUMMARY_ITEM4 = null;
            model.SUMMARY_ITEM5 = null;
            model.NOTE = null;

            Workflow.Controller.ValidateSave<Workflow.C测井现场提交信息>(model.JOB_PLAN_CD);

            StringBuilder strSql = new StringBuilder();
            List<OracleParameter> parameters = new List<OracleParameter>();
            if (string.IsNullOrWhiteSpace(model.CLASS_MEET_ID))
            {
                strSql.Append("insert into DM_LOG_CLASS_MEET (");
                strSql.Append("CLASS_MEET_ID,JOB_PLAN_CD,HOST,WRITER,MEETING_DATE,MEETING_TIME,PARTICIPANTS,SUMMARY_ITEM1,SUMMARY_ITEM2,SUMMARY_ITEM3,SUMMARY_ITEM6,SUMMARY_ITEM4,SUMMARY_ITEM5,NOTE,ATTACH_INFO,ATTACH_FILE)");
                strSql.Append(" values (");
                strSql.Append("CLASS_MEET_ID_SEQ.nextval,:JOB_PLAN_CD,:HOST,:WRITER,:MEETING_DATE,:MEETING_TIME,:PARTICIPANTS,:SUMMARY_ITEM1,:SUMMARY_ITEM2,:SUMMARY_ITEM3,:SUMMARY_ITEM6,:SUMMARY_ITEM4,:SUMMARY_ITEM5,:NOTE,:ATTACH_INFO,:ATTACH_FILE) ");
                //parameters.Add(ServiceUtils.CreateOracleParameter(":CLASS_MEET_ID", OracleDbType.Varchar2, GetCLASS_MEET_ID()));
                parameters.Add(ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, model.JOB_PLAN_CD));
            }
            else
            {
                strSql.Append("update DM_LOG_CLASS_MEET set ");
                //strSql.Append("JOB_PLAN_CD=:JOB_PLAN_CD,");
                strSql.Append("HOST=:HOST,");
                strSql.Append("WRITER=:WRITER,");
                strSql.Append("MEETING_DATE=:MEETING_DATE,");
                strSql.Append("MEETING_TIME=:MEETING_TIME,");
                strSql.Append("PARTICIPANTS=:PARTICIPANTS,");
                strSql.Append("SUMMARY_ITEM1=:SUMMARY_ITEM1,");
                strSql.Append("SUMMARY_ITEM2=:SUMMARY_ITEM2,");
                strSql.Append("SUMMARY_ITEM3=:SUMMARY_ITEM3,");
                strSql.Append("SUMMARY_ITEM6=:SUMMARY_ITEM6,");
                strSql.Append("SUMMARY_ITEM4=:SUMMARY_ITEM4,");
                strSql.Append("SUMMARY_ITEM5=:SUMMARY_ITEM5,");
                strSql.Append("NOTE=:NOTE,");
                //ADD
                strSql.Append("ATTACH_INFO=:ATTACH_INFO,");
                strSql.Append("ATTACH_FILE=:ATTACH_FILE");
                strSql.Append(" where CLASS_MEET_ID=:CLASS_MEET_ID ");
                parameters.Add(ServiceUtils.CreateOracleParameter(":CLASS_MEET_ID", OracleDbType.Char, model.CLASS_MEET_ID));
            }
            parameters.AddRange(new OracleParameter[] {    
                ServiceUtils.CreateOracleParameter(":HOST", OracleDbType.Char, model.HOST) ,
                ServiceUtils.CreateOracleParameter(":WRITER", OracleDbType.Char, model.WRITER) ,  
                        ServiceUtils.CreateOracleParameter(":MEETING_DATE", OracleDbType.Date, model.MEETING_DATE) ,            
                        ServiceUtils.CreateOracleParameter(":MEETING_TIME", OracleDbType.Decimal, model.MEETING_TIME) ,            
                        ServiceUtils.CreateOracleParameter(":PARTICIPANTS", OracleDbType.Char, model.PARTICIPANTS) ,            
                        ServiceUtils.CreateOracleParameter(":SUMMARY_ITEM1", OracleDbType.Char, model.SUMMARY_ITEM1) ,            
                        ServiceUtils.CreateOracleParameter(":SUMMARY_ITEM2", OracleDbType.Char, model.SUMMARY_ITEM2) ,            
                        ServiceUtils.CreateOracleParameter(":SUMMARY_ITEM3", OracleDbType.Char, model.SUMMARY_ITEM3) ,            
                        ServiceUtils.CreateOracleParameter(":SUMMARY_ITEM6", OracleDbType.Char, model.SUMMARY_ITEM6) ,            
                        ServiceUtils.CreateOracleParameter(":SUMMARY_ITEM4", OracleDbType.Char, model.SUMMARY_ITEM4) ,  
                        ServiceUtils.CreateOracleParameter(":SUMMARY_ITEM5", OracleDbType.Char, model.SUMMARY_ITEM5) , 
                        ServiceUtils.CreateOracleParameter(":NOTE", OracleDbType.Varchar2, model.NOTE),
                        ServiceUtils.CreateOracleParameter(":ATTACH_INFO", OracleDbType.Varchar2, model.ATTACH_INFO),
                        ServiceUtils.CreateOracleParameter(":ATTACH_FILE", OracleDbType.Blob, model.ATTACH_FILE)
            });

            if (DbHelperOra.ExecuteSqlTran(strSql.ToString(), parameters.ToArray()) > 0) return true;
            return false;
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetData_井场班前会(string string_作业计划书编号)
        {
            if (string.IsNullOrWhiteSpace(string_作业计划书编号)) return null;
            return DbHelperOra.Query("select * from DM_LOG_CLASS_MEET where job_plan_cd=:JOB_PLAN_CD",
                new OracleParameter[] { ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, string_作业计划书编号) });
        }
        #endregion
        #region 三交会DM_LOG_THREE_CROSS
        //public string GetTHREE_CROSS_ID()
        //{
        //    //DataTable dt = DbHelperOra.Query("select THREE_CROSS_ID  From DM_LOG_THREE_CROSS");
        //    //var max = dt.Select().Max(p => p["THREE_CROSS_ID"]);
        //    //int mum = Convert.ToInt32(max.ToString());
        //    //DataRow row = dt.Rows[0];
        //    //int n = Convert.ToInt32(row["THREE_CROSS_ID"]);
        //    var num = DbHelperOra.GetSingle(" select  max(to_number(THREE_CROSS_ID))+1  From  DM_LOG_THREE_CROSS ");
        //    return num == null ? "1" : num.ToString();
        //}

        [WebMethod(EnableSession = true)]
        public bool Save_三交会(byte[] data_三交会)
        {
            if (data_三交会 == null) return false;
            Model.DM_LOG_THREE_CROSS model = Utility.ModelHelper.DeserializeObject(data_三交会) as Model.DM_LOG_THREE_CROSS;
            if (model == null) return false;
            model.SUMMARY_ITEM6 = null;
            model.SUMMARY_ITEM4 = null;
            model.SUMMARY_ITEM5 = null;
            model.NOTE = null;

            Workflow.Controller.ValidateSave<Workflow.C测井现场提交信息>(model.JOB_PLAN_CD);

            StringBuilder strSql = new StringBuilder();
            List<OracleParameter> parameters = new List<OracleParameter>();
            if (string.IsNullOrWhiteSpace(model.THREE_CROSS_ID))
            {
                strSql.Append("insert into DM_LOG_THREE_CROSS (");
                strSql.Append("THREE_CROSS_ID,JOB_PLAN_CD,MEETING_DATE,MEETING_TIME,HOST,WRITER,PARTICIPANTS,SUMMARY_ITEM1,SUMMARY_ITEM2,SUMMARY_ITEM3,SUMMARY_ITEM6,SUMMARY_ITEM4,SUMMARY_ITEM5,NOTE,ATTACH_INFO,ATTACH_FILE)");
                strSql.Append(" values (");
                strSql.Append("THREE_CROSS_ID_SEQ.nextval,:JOB_PLAN_CD,:MEETING_DATE,:MEETING_TIME,:HOST,:WRITER,:PARTICIPANTS,:SUMMARY_ITEM1,:SUMMARY_ITEM2,:SUMMARY_ITEM3,:SUMMARY_ITEM6,:SUMMARY_ITEM4,:SUMMARY_ITEM5,:NOTE,:ATTACH_INFO,:ATTACH_FILE) ");
                //parameters.Add(ServiceUtils.CreateOracleParameter(":THREE_CROSS_ID", OracleDbType.Varchar2, GetTHREE_CROSS_ID()));
                parameters.Add(ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, model.JOB_PLAN_CD));
            }
            else
            {
                strSql.Append("update DM_LOG_THREE_CROSS set ");
                //strSql.Append("JOB_PLAN_CD=:JOB_PLAN_CD,");
                strSql.Append("MEETING_DATE=:MEETING_DATE,");
                strSql.Append("MEETING_TIME=:MEETING_TIME,");
                strSql.Append("HOST=:HOST,");
                strSql.Append("WRITER=:WRITER,");
                strSql.Append("PARTICIPANTS=:PARTICIPANTS,");
                strSql.Append("SUMMARY_ITEM1=:SUMMARY_ITEM1,");
                strSql.Append("SUMMARY_ITEM2=:SUMMARY_ITEM2,");
                strSql.Append("SUMMARY_ITEM3=:SUMMARY_ITEM3,");
                strSql.Append("SUMMARY_ITEM6=:SUMMARY_ITEM6,");
                strSql.Append("SUMMARY_ITEM4=:SUMMARY_ITEM4,");
                strSql.Append("SUMMARY_ITEM5=:SUMMARY_ITEM5,");
                strSql.Append("NOTE=:NOTE, ");
                //ADD
                strSql.Append("ATTACH_INFO=:ATTACH_INFO,");
                strSql.Append("ATTACH_FILE=:ATTACH_FILE ");
                strSql.Append(" where THREE_CROSS_ID=:THREE_CROSS_ID ");
                parameters.Add(ServiceUtils.CreateOracleParameter(":THREE_CROSS_ID", OracleDbType.Char, model.THREE_CROSS_ID));
            }
            parameters.AddRange(new OracleParameter[] {          
                        ServiceUtils.CreateOracleParameter(":MEETING_DATE", OracleDbType.Date, model.MEETING_DATE) ,            
                        ServiceUtils.CreateOracleParameter(":MEETING_TIME", OracleDbType.Decimal, model.MEETING_TIME) ,            
                        ServiceUtils.CreateOracleParameter(":HOST", OracleDbType.Char, model.HOST) ,            
                        ServiceUtils.CreateOracleParameter(":WRITER", OracleDbType.Char, model.WRITER) ,            
                        ServiceUtils.CreateOracleParameter(":PARTICIPANTS", OracleDbType.Char, model.PARTICIPANTS) ,            
                        ServiceUtils.CreateOracleParameter(":SUMMARY_ITEM1", OracleDbType.Char, model.SUMMARY_ITEM1) ,            
                        ServiceUtils.CreateOracleParameter(":SUMMARY_ITEM2", OracleDbType.Char, model.SUMMARY_ITEM2) ,            
                        ServiceUtils.CreateOracleParameter(":SUMMARY_ITEM3", OracleDbType.Char, model.SUMMARY_ITEM3) ,   
                        ServiceUtils.CreateOracleParameter(":SUMMARY_ITEM6", OracleDbType.Char, model.SUMMARY_ITEM6) ,            
                        ServiceUtils.CreateOracleParameter(":SUMMARY_ITEM4", OracleDbType.Char, model.SUMMARY_ITEM4) ,            
                        ServiceUtils.CreateOracleParameter(":SUMMARY_ITEM5", OracleDbType.Char, model.SUMMARY_ITEM5) ,
                        ServiceUtils.CreateOracleParameter(":NOTE", OracleDbType.Varchar2, model.NOTE),
                        ServiceUtils.CreateOracleParameter(":ATTACH_INFO",OracleDbType.Varchar2,model.ATTACH_INFO),
                        ServiceUtils.CreateOracleParameter(":ATTACH_FILE",OracleDbType.Blob,model.ATTACH_FILE)
            });
            if (DbHelperOra.ExecuteSqlTran(strSql.ToString(), parameters.ToArray()) > 0) return true;
            return false;
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetData_三交会(string string_作业计划书编号)
        {
            if (string.IsNullOrWhiteSpace(string_作业计划书编号)) return null;
            return DbHelperOra.Query("select * from DM_LOG_THREE_CROSS where job_plan_cd=:JOB_PLAN_CD",
                new OracleParameter[] { ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, string_作业计划书编号) });
        }
        #endregion
        #region 取芯参数PRO_LOG_CORE
        ///// <summary>
        ///// 获取取心ID
        ///// </summary>
        ///// <returns></returns>
        //[WebMethod(EnableSession = true)]
        //public string GetCOREID()
        //{
        //    var num = DbHelperOra.GetSingle("select max(to_number(COREID))  From PRO_LOG_CORE");
        //    return num == null ? "1" : (Convert.ToInt32(num.ToString()) + 1).ToString();
        //}
        [WebMethod(EnableSession = true)]
        public bool Savedata_取心参数(byte[] data_取心参数)
        {
            if (data_取心参数 == null) return false;
            Model.PRO_LOG_CORE model = Utility.ModelHelper.DeserializeObject(data_取心参数) as Model.PRO_LOG_CORE;
            if (model == null) return false;
            StringBuilder strSql = new StringBuilder();
            List<OracleParameter> parameters = new List<OracleParameter>();
            if (model.COREID == null)
            {
                strSql.Append("insert into PRO_LOG_CORE(");
                strSql.Append("COREID,JOB_PLAN_CD,REQUISITION_CD,UPDATE_DATE,FORMATION_NAME,START_DEPTH,END_DEPTH,CORE_WORD_DESCRIPTION,CORE_GR_DATA,CORE_PICTURE_DESCRIPTION");
                strSql.Append(") values (");
                strSql.Append("COREID_SEQ.nextval,:JOB_PLAN_CD,:REQUISITION_CD,:UPDATE_DATE,:FORMATION_NAME,:START_DEPTH,:END_DEPTH,:CORE_WORD_DESCRIPTION,:CORE_GR_DATA,:CORE_PICTURE_DESCRIPTION");
                strSql.Append(") ");
                //parameters.Add(ServiceUtils.CreateOracleParameter(":COREID", OracleDbType.Varchar2, GetCOREID()));
                parameters.Add(ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, model.JOB_PLAN_CD));
                parameters.Add(ServiceUtils.CreateOracleParameter(":REQUISITION_CD", OracleDbType.Varchar2, model.REQUISITION_CD));
            }
            else
            {
                strSql.Append("update PRO_LOG_CORE set ");
                //strSql.Append(" CONTACTID = :CONTACTID , ");
                //strSql.Append(" JOB_PLAN_CD = :JOB_PLAN_CD , ");
                //strSql.Append(" REQUISITION_CD = :REQUISITION_CD , ");
                strSql.Append(" UPDATE_DATE = :UPDATE_DATE, ");
                strSql.Append(" FORMATION_NAME = :FORMATION_NAME, ");
                strSql.Append(" START_DEPTH = :START_DEPTH , ");
                strSql.Append(" END_DEPTH = :END_DEPTH, ");
                strSql.Append(" CORE_WORD_DESCRIPTION = :CORE_WORD_DESCRIPTION, ");
                strSql.Append(" CORE_GR_DATA = :CORE_GR_DATA, ");
                strSql.Append(" CORE_PICTURE_DESCRIPTION = :CORE_PICTURE_DESCRIPTION ");
                strSql.Append(" where JOB_PLAN_CD=:JOB_PLAN_CD and COREID=:COREID1  ");
                parameters.Add(ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, model.JOB_PLAN_CD));
                parameters.Add(ServiceUtils.CreateOracleParameter(":COREID1", OracleDbType.Varchar2, model.COREID));
            }
            parameters.AddRange(new OracleParameter[] {          
                        ServiceUtils.CreateOracleParameter(":UPDATE_DATE", OracleDbType.Date, model.UPDATE_DATE) ,            
                        ServiceUtils.CreateOracleParameter(":FORMATION_NAME", OracleDbType.Varchar2, model.FORMATION_NAME) ,            
                        ServiceUtils.CreateOracleParameter(":START_DEPTH", OracleDbType.Decimal, model.START_DEPTH) ,            
                        ServiceUtils.CreateOracleParameter(":END_DEPTH", OracleDbType.Decimal, model.END_DEPTH) ,            
                        ServiceUtils.CreateOracleParameter(":CORE_WORD_DESCRIPTION", OracleDbType.NVarchar2, model.CORE_WORD_DESCRIPTION),
                        ServiceUtils.CreateOracleParameter(":CORE_GR_DATA", OracleDbType.Blob, model.CORE_GR_DATA) ,            
                        ServiceUtils.CreateOracleParameter(":CORE_PICTURE_DESCRIPTION", OracleDbType.NVarchar2, model.CORE_PICTURE_DESCRIPTION)

                         });
            if (DbHelperOra.ExecuteSqlTran(strSql.ToString(), parameters.ToArray()) > 0) return true;
            return false;
        }
        /// <summary>
        /// 获取取心参数列表
        /// </summary>
        /// <param name="string_计划任务书编号"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public DataSet GetDataGList_取心参数(string string_计划任务书编号)
        {
            if (string.IsNullOrWhiteSpace(string_计划任务书编号)) return null;
            return DbHelperOra.Query("select * from PRO_LOG_CORE where JOB_PLAN_CD=:JOB_PLAN_CD", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, string_计划任务书编号) });
        }
        /// <summary>
        /// 获取取心参数列表
        /// </summary>
        /// <param name="string_计划任务书编号"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public DataSet GetData_取心参数(string COREID)
        {
            if (string.IsNullOrWhiteSpace(COREID)) return null;
            return DbHelperOra.Query("select * from PRO_LOG_CORE where COREID=:COREID", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":COREID", OracleDbType.Varchar2, COREID) });
        }
        #endregion
        #region 井试油参数PRO_LOG_TESTOIL
        ///// <summary>
        ///// 获取井试油参数ID
        ///// </summary>
        ///// <returns></returns>
        //[WebMethod(EnableSession = true)]
        //public string GetTESTOILID()
        //{
        //    var num = DbHelperOra.GetSingle("select max(to_number(TESTOILID))  From PRO_LOG_TESTOIL");
        //    return num == null ? "1" : (Convert.ToInt32(num.ToString()) + 1).ToString();
        //}
        [WebMethod(EnableSession = true)]
        public bool Savedata_井试油参数(byte[] data_井试油参数)
        {
            if (data_井试油参数 == null) return false;
            Model.PRO_LOG_TESTOIL model = Utility.ModelHelper.DeserializeObject(data_井试油参数) as Model.PRO_LOG_TESTOIL;
            if (model == null) return false;
            StringBuilder strSql = new StringBuilder();
            List<OracleParameter> parameters = new List<OracleParameter>();
            if (model.TESTOILID == null)
            {
                strSql.Append("insert into PRO_LOG_TESTOIL(");
                strSql.Append("TESTOILID,JOB_PLAN_CD,REQUISITION_CD,TEST_DATE,LAYER_NAME,START_DEPTH,END_DEPTH,OUTPUT_OIL_EVERY_DAY,OUTPUT_WATER_EVERY_DAY,OUTPUT_GAS_EVERY_DAY,HYDROSULFIDE,FORMATION_PRESSURE,CHOKE,OIL_PRESSURE,CASING_PRESSURE,TEST_CONCLUSION,DETAILED_DESCRIPTION_TEST,NOTE)");
                strSql.Append(" values (");
                strSql.Append("TESTOILID_SEQ.nextval,:JOB_PLAN_CD,:REQUISITION_CD,:TEST_DATE,:LAYER_NAME,:START_DEPTH,:END_DEPTH,:OUTPUT_OIL_EVERY_DAY,:OUTPUT_WATER_EVERY_DAY,:OUTPUT_GAS_EVERY_DAY,:HYDROSULFIDE,:FORMATION_PRESSURE,:CHOKE,:OIL_PRESSURE,:CASING_PRESSURE,:TEST_CONCLUSION,:DETAILED_DESCRIPTION_TEST,:NOTE)");
                //parameters.Add(ServiceUtils.CreateOracleParameter(":TESTOILID", OracleDbType.Char, GetTESTOILID()));
                parameters.Add(ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, model.JOB_PLAN_CD));
                parameters.Add(ServiceUtils.CreateOracleParameter(":REQUISITION_CD", OracleDbType.NVarchar2, model.REQUISITION_CD));
            }
            else
            {
                strSql.Append("update PRO_LOG_TESTOIL set ");
                //strSql.Append("JOB_PLAN_CD=:JOB_PLAN_CD,");
                //strSql.Append("REQUISITION_CD=:REQUISITION_CD,");
                strSql.Append("TEST_DATE=:TEST_DATE,");
                strSql.Append("LAYER_NAME=:LAYER_NAME,");
                strSql.Append("START_DEPTH=:START_DEPTH,");
                strSql.Append("END_DEPTH=:END_DEPTH,");
                strSql.Append("OUTPUT_OIL_EVERY_DAY=:OUTPUT_OIL_EVERY_DAY,");
                strSql.Append("OUTPUT_WATER_EVERY_DAY=:OUTPUT_WATER_EVERY_DAY,");
                strSql.Append("OUTPUT_GAS_EVERY_DAY=:OUTPUT_GAS_EVERY_DAY,");
                strSql.Append("HYDROSULFIDE=:HYDROSULFIDE,");
                strSql.Append("FORMATION_PRESSURE=:FORMATION_PRESSURE,");
                strSql.Append("CHOKE=:CHOKE,");
                strSql.Append("OIL_PRESSURE=:OIL_PRESSURE,");
                strSql.Append("CASING_PRESSURE=:CASING_PRESSURE,");
                strSql.Append("TEST_CONCLUSION=:TEST_CONCLUSION,");
                strSql.Append("DETAILED_DESCRIPTION_TEST=:DETAILED_DESCRIPTION_TEST,");
                strSql.Append("NOTE=:NOTE");
                strSql.Append(" where JOB_PLAN_CD=:JOB_PLAN_CD and TESTOILID=:TESTOILID ");
                parameters.Add(ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, model.JOB_PLAN_CD));
                parameters.Add(ServiceUtils.CreateOracleParameter(":TESTOILID", OracleDbType.Char, model.TESTOILID));
            }
            parameters.AddRange(new OracleParameter[] {          
                        ServiceUtils.CreateOracleParameter(":TEST_DATE", OracleDbType.Date, model.TEST_DATE) ,            
                        ServiceUtils.CreateOracleParameter(":LAYER_NAME", OracleDbType.NVarchar2, model.LAYER_NAME) ,            
                        ServiceUtils.CreateOracleParameter(":START_DEPTH", OracleDbType.Decimal, model.START_DEPTH) ,            
                        ServiceUtils.CreateOracleParameter(":END_DEPTH", OracleDbType.Decimal, model.END_DEPTH) ,            
                        ServiceUtils.CreateOracleParameter(":OUTPUT_OIL_EVERY_DAY", OracleDbType.Decimal, model.OUTPUT_OIL_EVERY_DAY),
                        ServiceUtils.CreateOracleParameter(":OUTPUT_WATER_EVERY_DAY", OracleDbType.Decimal, model.OUTPUT_WATER_EVERY_DAY) ,  
                        ServiceUtils.CreateOracleParameter(":OUTPUT_GAS_EVERY_DAY", OracleDbType.Decimal, model.OUTPUT_GAS_EVERY_DAY) ,            
                        ServiceUtils.CreateOracleParameter(":HYDROSULFIDE", OracleDbType.Decimal, model.HYDROSULFIDE) ,            
                        ServiceUtils.CreateOracleParameter(":FORMATION_PRESSURE", OracleDbType.Decimal, model.FORMATION_PRESSURE) ,            
                        ServiceUtils.CreateOracleParameter(":CHOKE", OracleDbType.Decimal, model.CHOKE) ,            
                        ServiceUtils.CreateOracleParameter(":OIL_PRESSURE", OracleDbType.Decimal, model.OIL_PRESSURE),
                        ServiceUtils.CreateOracleParameter(":CASING_PRESSURE", OracleDbType.Decimal, model.CASING_PRESSURE) , 
                        ServiceUtils.CreateOracleParameter(":TEST_CONCLUSION", OracleDbType.NVarchar2, model.TEST_CONCLUSION),
                        ServiceUtils.CreateOracleParameter(":DETAILED_DESCRIPTION_TEST", OracleDbType.NVarchar2, model.DETAILED_DESCRIPTION_TEST) , 
                        ServiceUtils.CreateOracleParameter(":NOTE", OracleDbType.NVarchar2, model.NOTE)

                         });
            if (DbHelperOra.ExecuteSqlTran(strSql.ToString(), parameters.ToArray()) > 0) return true;
            return false;
        }
        /// <summary>
        /// 获取井试油参数列表
        /// </summary>
        /// <param name="string_计划任务书编号"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public DataSet GetDataGList_井试油参数(string string_计划任务书编号)
        {
            if (string.IsNullOrWhiteSpace(string_计划任务书编号)) return null;
            return DbHelperOra.Query("select * from PRO_LOG_TESTOIL where JOB_PLAN_CD=:JOB_PLAN_CD", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, string_计划任务书编号) });
        }
        /// <summary>
        /// 获取井试油参数列表
        /// </summary>
        /// <param name="string_计划任务书编号"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public DataSet GetData_井试油参数(string TESTOILID)
        {
            if (string.IsNullOrWhiteSpace(TESTOILID)) return null;
            return DbHelperOra.Query("select * from PRO_LOG_TESTOIL where TESTOILID=:TESTOILID", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":TESTOILID", OracleDbType.Char, TESTOILID) });
        }
        #endregion
        #region 钻井液参数PRO_LOG_SLOP
        ///// <summary>
        ///// 获取钻井液参数ID
        ///// </summary>
        ///// <returns></returns>
        //[WebMethod(EnableSession = true)]
        //public string MUDID()
        //{
        //    var num = DbHelperOra.GetSingle("select max(to_number(MUDID))  From PRO_LOG_SLOP");
        //    return num == null ? "1" : (Convert.ToInt32(num.ToString()) + 1).ToString();
        //}
        [WebMethod(EnableSession = true)]
        public bool Savedata_钻井液参数(byte[] data_钻井液参数)
        {
            if (data_钻井液参数 == null) return false;
            Model.PRO_LOG_SLOP model = Utility.ModelHelper.DeserializeObject(data_钻井液参数) as Model.PRO_LOG_SLOP;
            if (model == null) return false;
            StringBuilder strSql = new StringBuilder();
            List<OracleParameter> parameters = new List<OracleParameter>();
            if (model.MUDID == null)
            {
                strSql.Append("insert into PRO_LOG_SLOP(");
                strSql.Append("MUDID,JOB_PLAN_CD,REQUISITION_CD,UPDATE_DATE,JTLT,SLOP_PROPERTIES,P_XN_MUD_DENSITY,SLOP_PERSENT,SLOP_PH,DRILL_FLU_VISC,SLOP_TEMPERATURE,SLOP_RESISTIVITY,DRILLING_FLUID_SALINITY,MUD_FILTRATE_SALINITY,CAKE_DENSITY)");
                strSql.Append(" values (");
                strSql.Append("MUDID_SEQ.nextval,:JOB_PLAN_CD,:REQUISITION_CD,:UPDATE_DATE,:JTLT,:SLOP_PROPERTIES,:P_XN_MUD_DENSITY,:SLOP_PERSENT,:SLOP_PH,:DRILL_FLU_VISC,:SLOP_TEMPERATURE,:SLOP_RESISTIVITY,:DRILLING_FLUID_SALINITY,:MUD_FILTRATE_SALINITY,:CAKE_DENSITY)");

                //parameters.Add(ServiceUtils.CreateOracleParameter(":MUDID", OracleDbType.Varchar2, MUDID()));
                parameters.Add(ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, model.JOB_PLAN_CD));
                parameters.Add(ServiceUtils.CreateOracleParameter(":REQUISITION_CD", OracleDbType.NVarchar2, model.REQUISITION_CD));
            }
            else
            {
                strSql.Append("update PRO_LOG_SLOP set ");
                //strSql.Append("JOB_PLAN_CD=:JOB_PLAN_CD,");
                //strSql.Append("REQUISITION_CD=:REQUISITION_CD,");
                strSql.Append("UPDATE_DATE=:UPDATE_DATE,");
                strSql.Append("JTLT=:JTLT,");
                strSql.Append("SLOP_PROPERTIES=:SLOP_PROPERTIES,");
                strSql.Append("P_XN_MUD_DENSITY=:P_XN_MUD_DENSITY,");
                strSql.Append("SLOP_PERSENT=:SLOP_PERSENT,");
                strSql.Append("SLOP_PH=:SLOP_PH,");
                strSql.Append("DRILL_FLU_VISC=:DRILL_FLU_VISC,");
                strSql.Append("SLOP_TEMPERATURE=:SLOP_TEMPERATURE,");
                strSql.Append("SLOP_RESISTIVITY=:SLOP_RESISTIVITY,");
                strSql.Append("DRILLING_FLUID_SALINITY=:DRILLING_FLUID_SALINITY,");
                strSql.Append("MUD_FILTRATE_SALINITY=:MUD_FILTRATE_SALINITY,");
                strSql.Append("CAKE_DENSITY=:CAKE_DENSITY");
                strSql.Append(" where JOB_PLAN_CD=:JOB_PLAN_CD and MUDID=:MUDID ");
                parameters.Add(ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, model.JOB_PLAN_CD));
                parameters.Add(ServiceUtils.CreateOracleParameter(":MUDID", OracleDbType.Varchar2, model.MUDID));
            }
            parameters.AddRange(new OracleParameter[] {          
                        ServiceUtils.CreateOracleParameter(":UPDATE_DATE", OracleDbType.Date, model.UPDATE_DATE) ,            
                        ServiceUtils.CreateOracleParameter(":JTLT", OracleDbType.Varchar2, model.JTLT) ,            
                        ServiceUtils.CreateOracleParameter(":SLOP_PROPERTIES", OracleDbType.Varchar2, model.SLOP_PROPERTIES) ,            
                        ServiceUtils.CreateOracleParameter(":P_XN_MUD_DENSITY", OracleDbType.Decimal, model.P_XN_MUD_DENSITY) ,            
                        ServiceUtils.CreateOracleParameter(":SLOP_PERSENT", OracleDbType.Decimal, model.SLOP_PERSENT),
                        ServiceUtils.CreateOracleParameter(":SLOP_PH", OracleDbType.Decimal, model.SLOP_PH) ,   
                        ServiceUtils.CreateOracleParameter(":DRILL_FLU_VISC", OracleDbType.Decimal, model.DRILL_FLU_VISC) ,            
                        ServiceUtils.CreateOracleParameter(":SLOP_TEMPERATURE", OracleDbType.Decimal, model.SLOP_TEMPERATURE) ,            
                        ServiceUtils.CreateOracleParameter(":SLOP_RESISTIVITY", OracleDbType.Decimal, model.SLOP_RESISTIVITY) ,            
                        ServiceUtils.CreateOracleParameter(":DRILLING_FLUID_SALINITY", OracleDbType.Decimal, model.DRILLING_FLUID_SALINITY) ,            
                        ServiceUtils.CreateOracleParameter(":MUD_FILTRATE_SALINITY", OracleDbType.Decimal, model.MUD_FILTRATE_SALINITY),
                        ServiceUtils.CreateOracleParameter(":CAKE_DENSITY", OracleDbType.Decimal, model.CAKE_DENSITY)

                         });
            if (DbHelperOra.ExecuteSqlTran(strSql.ToString(), parameters.ToArray()) > 0) return true;
            return false;
        }
        /// <summary>
        /// 获取钻井液参数列表
        /// </summary>
        /// <param name="string_计划任务书编号"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public DataSet GetDataGList_钻井液参数(string string_计划任务书编号)
        {
            if (string.IsNullOrWhiteSpace(string_计划任务书编号)) return null;
            return DbHelperOra.Query("select * from PRO_LOG_SLOP where JOB_PLAN_CD=:JOB_PLAN_CD", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, string_计划任务书编号) });
        }
        /// <summary>
        /// 获取钻井液参数列表
        /// </summary>
        /// <param name="string_计划任务书编号"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public DataSet GetData_钻井液参数(string MUDID)
        {
            if (string.IsNullOrWhiteSpace(MUDID)) return null;
            return DbHelperOra.Query("select * from PRO_LOG_SLOP where MUDID=:MUDID", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":MUDID", OracleDbType.Varchar2, MUDID) });
        }
        #endregion
        #region 生产参数PRO_LOG_PRODUCE
        ///// <summary>
        ///// 获取生产参数ID
        ///// </summary>
        ///// <returns></returns>
        //[WebMethod(EnableSession = true)]
        //public string GetWORKID()
        //{
        //    var num = DbHelperOra.GetSingle("select max(to_number(WORKID))  From PRO_LOG_PRODUCE");
        //    return num == null ? "1" : (Convert.ToInt32(num.ToString()) + 1).ToString();
        //}
        [WebMethod(EnableSession = true)]
        public bool Savedata_生产参数(byte[] data_生产参数)
        {
            if (data_生产参数 == null) return false;
            Model.PRO_LOG_PRODUCE model = Utility.ModelHelper.DeserializeObject(data_生产参数) as Model.PRO_LOG_PRODUCE;
            if (model == null) return false;
            StringBuilder strSql = new StringBuilder();
            List<OracleParameter> parameters = new List<OracleParameter>();
            if (model.WORKID == null)
            {
                strSql.Append("insert into PRO_LOG_PRODUCE(");
                strSql.Append("WORKID,JOB_PLAN_CD,REQUISITION_CD,UPDATE_DATE,OIL_SHOE_DEPTH,WELL_FLUID_TYPE,OIL_LINE_INNER_DIAMETER,BELLMOUTH_DEPTH,CHOKE,SUL_HYD,WELLHEAD_PRESS,WELLBOTTOM_PRESS,WELLHEAD_FLANGE_TYPE,WATER_PRO_PER_DAY,AIR_DAY_PRODUCTION,OIL_PRO_PER_DAY,CASING_PRESSURE,OIL_PRESSURE,NOTE)");
                strSql.Append(" values (");
                strSql.Append("WORKID_SEQ.nextval,:JOB_PLAN_CD,:REQUISITION_CD,:UPDATE_DATE,:OIL_SHOE_DEPTH,:WELL_FLUID_TYPE,:OIL_LINE_INNER_DIAMETER,:BELLMOUTH_DEPTH,:CHOKE,:SUL_HYD,:WELLHEAD_PRESS,:WELLBOTTOM_PRESS,:WELLHEAD_FLANGE_TYPE,:WATER_PRO_PER_DAY,:AIR_DAY_PRODUCTION,:OIL_PRO_PER_DAY,:CASING_PRESSURE,:OIL_PRESSURE,:NOTE)");

                //parameters.Add(ServiceUtils.CreateOracleParameter(":WORKID", OracleDbType.Varchar2, GetWORKID()));
                parameters.Add(ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, model.JOB_PLAN_CD));
                parameters.Add(ServiceUtils.CreateOracleParameter(":REQUISITION_CD", OracleDbType.NVarchar2, model.REQUISITION_CD));
            }
            else
            {
                strSql.Append("update PRO_LOG_PRODUCE set ");
                //strSql.Append("JOB_PLAN_CD=:JOB_PLAN_CD,");
                //strSql.Append("REQUISITION_CD=:REQUISITION_CD,");
                strSql.Append("UPDATE_DATE=:UPDATE_DATE,");
                strSql.Append("OIL_SHOE_DEPTH=:OIL_SHOE_DEPTH,");
                strSql.Append("WELL_FLUID_TYPE=:WELL_FLUID_TYPE,");
                strSql.Append("OIL_LINE_INNER_DIAMETER=:OIL_LINE_INNER_DIAMETER,");
                strSql.Append("BELLMOUTH_DEPTH=:BELLMOUTH_DEPTH,");
                strSql.Append("CHOKE=:CHOKE,");
                strSql.Append("SUL_HYD=:SUL_HYD,");
                strSql.Append("WELLHEAD_PRESS=:WELLHEAD_PRESS,");
                strSql.Append("WELLBOTTOM_PRESS=:WELLBOTTOM_PRESS,");
                strSql.Append("WELLHEAD_FLANGE_TYPE=:WELLHEAD_FLANGE_TYPE,");
                strSql.Append("WATER_PRO_PER_DAY=:WATER_PRO_PER_DAY,");
                strSql.Append("AIR_DAY_PRODUCTION=:AIR_DAY_PRODUCTION,");
                strSql.Append("OIL_PRO_PER_DAY=:OIL_PRO_PER_DAY,");
                strSql.Append("CASING_PRESSURE=:CASING_PRESSURE,");
                strSql.Append("OIL_PRESSURE=:OIL_PRESSURE,");
                strSql.Append("NOTE=:NOTE");
                strSql.Append(" where JOB_PLAN_CD=:JOB_PLAN_CD and WORKID=:WORKID ");
                parameters.Add(ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, model.JOB_PLAN_CD));
                parameters.Add(ServiceUtils.CreateOracleParameter(":WORKID", OracleDbType.Varchar2, model.WORKID));
            }
            parameters.AddRange(new OracleParameter[] {          
                        ServiceUtils.CreateOracleParameter(":UPDATE_DATE", OracleDbType.Date, model.UPDATE_DATE) ,            
                        ServiceUtils.CreateOracleParameter(":OIL_SHOE_DEPTH", OracleDbType.Decimal, model.OIL_SHOE_DEPTH) ,            
                        ServiceUtils.CreateOracleParameter(":WELL_FLUID_TYPE", OracleDbType.Varchar2, model.WELL_FLUID_TYPE) ,            
                        ServiceUtils.CreateOracleParameter(":OIL_LINE_INNER_DIAMETER", OracleDbType.Decimal, model.OIL_LINE_INNER_DIAMETER) ,            
                        ServiceUtils.CreateOracleParameter(":BELLMOUTH_DEPTH", OracleDbType.Decimal, model.BELLMOUTH_DEPTH),
                        ServiceUtils.CreateOracleParameter(":CHOKE", OracleDbType.Decimal, model.CHOKE) ,            
                        ServiceUtils.CreateOracleParameter(":SUL_HYD", OracleDbType.Decimal, model.SUL_HYD),
                         ServiceUtils.CreateOracleParameter(":WELLHEAD_PRESS", OracleDbType.Decimal, model.WELLHEAD_PRESS) ,            
                        ServiceUtils.CreateOracleParameter(":WELLBOTTOM_PRESS", OracleDbType.Decimal, model.WELLBOTTOM_PRESS) ,  
                        ServiceUtils.CreateOracleParameter(":WELLHEAD_FLANGE_TYPE", OracleDbType.Varchar2, model.WELLHEAD_FLANGE_TYPE) ,            
                        ServiceUtils.CreateOracleParameter(":WATER_PRO_PER_DAY", OracleDbType.Decimal, model.WATER_PRO_PER_DAY) ,            
                        ServiceUtils.CreateOracleParameter(":AIR_DAY_PRODUCTION", OracleDbType.Decimal, model.AIR_DAY_PRODUCTION) ,            
                        ServiceUtils.CreateOracleParameter(":OIL_PRO_PER_DAY", OracleDbType.Decimal, model.OIL_PRO_PER_DAY) ,            
                        ServiceUtils.CreateOracleParameter(":CASING_PRESSURE", OracleDbType.Decimal, model.CASING_PRESSURE),
                        ServiceUtils.CreateOracleParameter(":OIL_PRESSURE", OracleDbType.Decimal, model.OIL_PRESSURE) ,            
                        ServiceUtils.CreateOracleParameter(":NOTE", OracleDbType.NVarchar2, model.NOTE)

                         });
            if (DbHelperOra.ExecuteSqlTran(strSql.ToString(), parameters.ToArray()) > 0) return true;
            return false;
        }
        /// <summary>
        /// 获取生产参数列表
        /// </summary>
        /// <param name="string_计划任务书编号"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public DataSet GetDataGList_生产参数(string string_计划任务书编号)
        {
            if (string.IsNullOrWhiteSpace(string_计划任务书编号)) return null;
            return DbHelperOra.Query("select * from PRO_LOG_PRODUCE where JOB_PLAN_CD=:JOB_PLAN_CD", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, string_计划任务书编号) });
        }
        /// <summary>
        /// 获取生产参数列表
        /// </summary>
        /// <param name="string_计划任务书编号"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public DataSet GetData_生产参数(string WORKID)
        {
            if (string.IsNullOrWhiteSpace(WORKID)) return null;
            return DbHelperOra.Query("select * from PRO_LOG_PRODUCE where WORKID=:WORKID", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":WORKID", OracleDbType.Varchar2, WORKID) });
        }
        #endregion
        #region 套管参数PRO_LOG_CASIN
        ///// <summary>
        ///// 获取套管参数ID
        ///// </summary>
        ///// <returns></returns>
        //[WebMethod(EnableSession = true)]
        //public string GetCASINID()
        //{
        //    var num = DbHelperOra.GetSingle("select max(to_number(CASINID))  From PRO_LOG_CASIN");
        //    return num == null ? "1" : (Convert.ToInt32(num.ToString()) + 1).ToString();
        //}
        [WebMethod(EnableSession = true)]
        public bool Savedata_套管参数(byte[] data_套管参数)
        {
            if (data_套管参数 == null) return false;
            Model.PRO_LOG_CASIN model = Utility.ModelHelper.DeserializeObject(data_套管参数) as Model.PRO_LOG_CASIN;
            if (model == null) return false;
            StringBuilder strSql = new StringBuilder();
            List<OracleParameter> parameters = new List<OracleParameter>();
            if (model.CASINID == null)
            {
                strSql.Append("insert into PRO_LOG_CASIN(");
                strSql.Append("CASINID,JOB_PLAN_CD,REQUISITION_CD,UPDATE_DATE,CASING_NAME,CASING_TYPE,CASING_OUTSIZE,CASING_QUANTITY,CASING_THREAD_TYPE,CASING_GRADE,PIPE_THICKNESS,P_XN_CASING_LENGTH,ACCUMULATIVE_LENGTH,RUNNING_BOTTOM_DEPTH,SCREW_ON_TORQUE,CENTERING_DEVICE_POSITION,CENTERING_DEVICE_NO,CENTERING_DEVICE_TYPE,CENTERING_DEVICE_SIZE,BASETUBE_BORESIZE,BASETUBE_OUTSIZE,BASETUBE_UNITNUMBER,BASETUBE_TYPE,BASETUBE_SLOTWIDTH,BASETUBE_POREDIAMETER,SIEVETUBE_SLOTWIDTH,SIEVETUBE_SLOTLENGTH,PIPE_HEIGHT,CURRENT_LOG_FILL,CURRENT_LOG_PLUGBACK,NOTE");
                strSql.Append(") values (");
                strSql.Append("CASINID_SEQ.nextval,:JOB_PLAN_CD,:REQUISITION_CD,:UPDATE_DATE,:CASING_NAME,:CASING_TYPE,:CASING_OUTSIZE,:CASING_QUANTITY,:CASING_THREAD_TYPE,:CASING_GRADE,:PIPE_THICKNESS,:P_XN_CASING_LENGTH,:ACCUMULATIVE_LENGTH,:RUNNING_BOTTOM_DEPTH,:SCREW_ON_TORQUE,:CENTERING_DEVICE_POSITION,:CENTERING_DEVICE_NO,:CENTERING_DEVICE_TYPE,:CENTERING_DEVICE_SIZE,:BASETUBE_BORESIZE,:BASETUBE_OUTSIZE,:BASETUBE_UNITNUMBER,:BASETUBE_TYPE,:BASETUBE_SLOTWIDTH,:BASETUBE_POREDIAMETER,:SIEVETUBE_SLOTWIDTH,:SIEVETUBE_SLOTLENGTH,:PIPE_HEIGHT,:CURRENT_LOG_FILL,:CURRENT_LOG_PLUGBACK,:NOTE");
                strSql.Append(") ");
                //parameters.Add(ServiceUtils.CreateOracleParameter(":CASINID", OracleDbType.Varchar2, GetCASINID()));
                parameters.Add(ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, model.JOB_PLAN_CD));
                parameters.Add(ServiceUtils.CreateOracleParameter(":REQUISITION_CD", OracleDbType.NVarchar2, model.REQUISITION_CD));
            }
            else
            {
                strSql.Append("update PRO_LOG_CASIN set ");
                //strSql.Append(" CASINID = :CASINID , ");
                //strSql.Append(" JOB_PLAN_CD = :JOB_PLAN_CD , ");
                //strSql.Append(" REQUISITION_CD = :REQUISITION_CD , ");
                strSql.Append(" UPDATE_DATE = :UPDATE_DATE , ");
                strSql.Append(" CASING_NAME = :CASING_NAME , ");
                strSql.Append(" CASING_TYPE = :CASING_TYPE , ");
                strSql.Append(" CASING_OUTSIZE = :CASING_OUTSIZE , ");
                strSql.Append(" CASING_QUANTITY = :CASING_QUANTITY , ");
                strSql.Append(" CASING_THREAD_TYPE = :CASING_THREAD_TYPE , ");
                strSql.Append(" CASING_GRADE = :CASING_GRADE , ");
                strSql.Append(" PIPE_THICKNESS = :PIPE_THICKNESS , ");
                strSql.Append(" P_XN_CASING_LENGTH = :P_XN_CASING_LENGTH , ");
                strSql.Append(" ACCUMULATIVE_LENGTH = :ACCUMULATIVE_LENGTH , ");
                strSql.Append(" RUNNING_BOTTOM_DEPTH = :RUNNING_BOTTOM_DEPTH , ");
                strSql.Append(" SCREW_ON_TORQUE = :SCREW_ON_TORQUE , ");
                strSql.Append(" CENTERING_DEVICE_POSITION = :CENTERING_DEVICE_POSITION , ");
                strSql.Append(" CENTERING_DEVICE_NO = :CENTERING_DEVICE_NO , ");
                strSql.Append(" CENTERING_DEVICE_TYPE = :CENTERING_DEVICE_TYPE , ");
                strSql.Append(" CENTERING_DEVICE_SIZE = :CENTERING_DEVICE_SIZE , ");
                strSql.Append(" BASETUBE_BORESIZE = :BASETUBE_BORESIZE , ");
                strSql.Append(" BASETUBE_OUTSIZE = :BASETUBE_OUTSIZE , ");
                strSql.Append(" BASETUBE_UNITNUMBER = :BASETUBE_UNITNUMBER , ");
                strSql.Append(" BASETUBE_TYPE = :BASETUBE_TYPE , ");
                strSql.Append(" BASETUBE_SLOTWIDTH = :BASETUBE_SLOTWIDTH , ");
                strSql.Append(" BASETUBE_POREDIAMETER = :BASETUBE_POREDIAMETER , ");
                strSql.Append(" SIEVETUBE_SLOTWIDTH = :SIEVETUBE_SLOTWIDTH , ");
                strSql.Append(" SIEVETUBE_SLOTLENGTH = :SIEVETUBE_SLOTLENGTH , ");
                strSql.Append(" PIPE_HEIGHT = :PIPE_HEIGHT , ");
                strSql.Append(" CURRENT_LOG_FILL = :CURRENT_LOG_FILL , ");
                strSql.Append(" CURRENT_LOG_PLUGBACK = :CURRENT_LOG_PLUGBACK , ");
                strSql.Append(" NOTE = :NOTE  ");
                strSql.Append(" where JOB_PLAN_CD=:JOB_PLAN_CD and CASINID=:CASINID ");
                parameters.Add(ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, model.JOB_PLAN_CD));
                parameters.Add(ServiceUtils.CreateOracleParameter(":CASINID", OracleDbType.Varchar2, model.CASINID));
            }
            parameters.AddRange(new OracleParameter[] {          
                        ServiceUtils.CreateOracleParameter(":UPDATE_DATE", OracleDbType.Date, model.UPDATE_DATE) ,            
                        ServiceUtils.CreateOracleParameter(":CASING_NAME", OracleDbType.Varchar2, model.CASING_NAME) ,            
                        ServiceUtils.CreateOracleParameter(":CASING_TYPE", OracleDbType.Varchar2, model.CASING_TYPE) ,
                        ServiceUtils.CreateOracleParameter(":CASING_OUTSIZE", OracleDbType.Decimal, model.CASING_OUTSIZE),
                        ServiceUtils.CreateOracleParameter(":CASING_QUANTITY", OracleDbType.Decimal, model.CASING_QUANTITY) , 
            ServiceUtils.CreateOracleParameter(":CASING_THREAD_TYPE", OracleDbType.Varchar2, model.CASING_THREAD_TYPE) ,            
                        ServiceUtils.CreateOracleParameter(":CASING_GRADE", OracleDbType.Varchar2, model.CASING_GRADE) ,            
                        ServiceUtils.CreateOracleParameter(":PIPE_THICKNESS", OracleDbType.Decimal, model.PIPE_THICKNESS) ,    
                         ServiceUtils.CreateOracleParameter(":P_XN_CASING_LENGTH", OracleDbType.Decimal, model.P_XN_CASING_LENGTH) ,            
                        ServiceUtils.CreateOracleParameter(":ACCUMULATIVE_LENGTH", OracleDbType.Decimal, model.ACCUMULATIVE_LENGTH) ,            
                        ServiceUtils.CreateOracleParameter(":RUNNING_BOTTOM_DEPTH", OracleDbType.Decimal, model.RUNNING_BOTTOM_DEPTH) ,    
                         ServiceUtils.CreateOracleParameter(":SCREW_ON_TORQUE", OracleDbType.Decimal, model.SCREW_ON_TORQUE) ,            
                        ServiceUtils.CreateOracleParameter(":CENTERING_DEVICE_POSITION", OracleDbType.Decimal, model.CENTERING_DEVICE_POSITION) ,            
                        ServiceUtils.CreateOracleParameter(":CENTERING_DEVICE_NO", OracleDbType.Decimal, model.CENTERING_DEVICE_NO) ,    
                         ServiceUtils.CreateOracleParameter(":CENTERING_DEVICE_TYPE", OracleDbType.Varchar2, model.CENTERING_DEVICE_TYPE) ,            
                        ServiceUtils.CreateOracleParameter(":CENTERING_DEVICE_SIZE", OracleDbType.Decimal, model.CENTERING_DEVICE_SIZE) ,            
                        ServiceUtils.CreateOracleParameter(":BASETUBE_BORESIZE", OracleDbType.Decimal, model.BASETUBE_BORESIZE) ,    
                        ServiceUtils.CreateOracleParameter(":BASETUBE_OUTSIZE", OracleDbType.Decimal, model.BASETUBE_OUTSIZE) ,            
                        ServiceUtils.CreateOracleParameter(":BASETUBE_UNITNUMBER", OracleDbType.Decimal, model.BASETUBE_UNITNUMBER) ,            
                        ServiceUtils.CreateOracleParameter(":BASETUBE_TYPE", OracleDbType.Varchar2, model.BASETUBE_TYPE) ,    
                        ServiceUtils.CreateOracleParameter(":BASETUBE_SLOTWIDTH", OracleDbType.Decimal, model.BASETUBE_SLOTWIDTH) ,            
                        ServiceUtils.CreateOracleParameter(":BASETUBE_POREDIAMETER", OracleDbType.Decimal, model.BASETUBE_POREDIAMETER) ,            
                        ServiceUtils.CreateOracleParameter(":SIEVETUBE_SLOTWIDTH", OracleDbType.Decimal, model.SIEVETUBE_SLOTWIDTH) ,    
                        ServiceUtils.CreateOracleParameter(":SIEVETUBE_SLOTLENGTH", OracleDbType.Decimal, model.SIEVETUBE_SLOTLENGTH) ,            
                        ServiceUtils.CreateOracleParameter(":PIPE_HEIGHT", OracleDbType.Decimal, model.PIPE_HEIGHT) ,            
                        ServiceUtils.CreateOracleParameter(":CURRENT_LOG_FILL", OracleDbType.Decimal, model.CURRENT_LOG_FILL) ,    
                        ServiceUtils.CreateOracleParameter(":CURRENT_LOG_PLUGBACK", OracleDbType.Decimal, model.CURRENT_LOG_PLUGBACK) ,            
                        ServiceUtils.CreateOracleParameter(":NOTE", OracleDbType.NVarchar2, model.NOTE)          
            });
            if (DbHelperOra.ExecuteSqlTran(strSql.ToString(), parameters.ToArray()) > 0) return true;
            return false;
        }
        /// <summary>
        /// 获取套管参数列表
        /// </summary>
        /// <param name="string_计划任务书编号"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public DataSet GetDataGList_套管参数(string string_计划任务书编号)
        {
            if (string.IsNullOrWhiteSpace(string_计划任务书编号)) return null;
            return DbHelperOra.Query("select * from PRO_LOG_CASIN where JOB_PLAN_CD=:JOB_PLAN_CD", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, string_计划任务书编号) });
        }
        /// <summary>
        /// 获取套管参数列表
        /// </summary>
        /// <param name="string_计划任务书编号"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public DataSet GetData_套管参数(string CASINID)
        {
            if (string.IsNullOrWhiteSpace(CASINID)) return null;
            return DbHelperOra.Query("select * from PRO_LOG_CASIN where CASINID=:CASINID", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":CASINID", OracleDbType.Varchar2, CASINID) });
        }
        #endregion
        #region 钻头程序PRO_LOG_BIT_PROGRAM
        ///// <summary>
        ///// 获取钻头ID
        ///// </summary>
        ///// <returns></returns>
        //[WebMethod(EnableSession = true)]
        //public string GetBITID()
        //{
        //    var num = DbHelperOra.GetSingle("select max(to_number(BITID))  From PRO_LOG_BIT_PROGRAM");
        //    return num == null ? "1" : (Convert.ToInt32(num.ToString()) + 1).ToString();
        //}
        [WebMethod(EnableSession = true)]
        public bool Savedata_钻头程序(byte[] data_钻头程序)
        {
            if (data_钻头程序 == null) return false;
            Model.PRO_LOG_BIT_PROGRAM model = Utility.ModelHelper.DeserializeObject(data_钻头程序) as Model.PRO_LOG_BIT_PROGRAM;
            if (model == null) return false;
            StringBuilder strSql = new StringBuilder();
            List<OracleParameter> parameters = new List<OracleParameter>();
            if (model.BITID == null)
            {
                strSql.Append("insert into PRO_LOG_BIT_PROGRAM(");
                strSql.Append("BITID,JOB_PLAN_CD,REQUISITION_CD,UPDATE_DATE,BIT_SIZE,BIT_DEP,NOTE)");
                strSql.Append(" values (");
                strSql.Append("BITID_SEQ.nextval,:JOB_PLAN_CD,:REQUISITION_CD,:UPDATE_DATE,:BIT_SIZE,:BIT_DEP,:NOTE)");
                //parameters.Add(ServiceUtils.CreateOracleParameter(":BITID", OracleDbType.Varchar2, GetBITID()));
                parameters.Add(ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, model.JOB_PLAN_CD));
                parameters.Add(ServiceUtils.CreateOracleParameter(":REQUISITION_CD", OracleDbType.NVarchar2, model.REQUISITION_CD));
            }
            else
            {
                strSql.Append("update PRO_LOG_BIT_PROGRAM set ");
                //strSql.Append("JOB_PLAN_CD=:JOB_PLAN_CD,");
                //strSql.Append("REQUISITION_CD=:REQUISITION_CD,");
                strSql.Append("UPDATE_DATE=:UPDATE_DATE,");
                strSql.Append("BIT_SIZE=:BIT_SIZE,");
                strSql.Append("BIT_DEP=:BIT_DEP,");
                strSql.Append("NOTE=:NOTE");
                strSql.Append(" where JOB_PLAN_CD=:JOB_PLAN_CD and BITID=:BITID ");
                parameters.Add(ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, model.JOB_PLAN_CD));
                parameters.Add(ServiceUtils.CreateOracleParameter(":BITID", OracleDbType.Varchar2, model.BITID));
            }
            parameters.AddRange(new OracleParameter[] {          
                        ServiceUtils.CreateOracleParameter(":UPDATE_DATE", OracleDbType.Date, model.UPDATE_DATE) ,            
                        ServiceUtils.CreateOracleParameter(":BIT_SIZE", OracleDbType.Decimal, model.BIT_SIZE) ,            
                        ServiceUtils.CreateOracleParameter(":BIT_DEP", OracleDbType.Decimal, model.BIT_DEP) ,                        
                        ServiceUtils.CreateOracleParameter(":NOTE", OracleDbType.NVarchar2, model.NOTE) });
            if (DbHelperOra.ExecuteSqlTran(strSql.ToString(), parameters.ToArray()) > 0) return true;
            return false;
        }
        /// <summary>
        /// 获取钻头程序列表
        /// </summary>
        /// <param name="string_计划任务书编号"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public DataSet GetDataGList_钻头程序(string string_计划任务书编号)
        {
            if (string.IsNullOrWhiteSpace(string_计划任务书编号)) return null;
            return DbHelperOra.Query("select * from PRO_LOG_BIT_PROGRAM where JOB_PLAN_CD=:JOB_PLAN_CD", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, string_计划任务书编号) });
        }
        /// <summary>
        /// 获取钻头程序列表
        /// </summary>
        /// <param name="string_计划任务书编号"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public DataSet GetData_钻头程序(string BITID)
        {
            if (string.IsNullOrWhiteSpace(BITID)) return null;
            return DbHelperOra.Query("select * from PRO_LOG_BIT_PROGRAM where BITID=:BITID", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":BITID", OracleDbType.Varchar2, BITID) });
        }
        #endregion
        #region 固井参数PRO_LOG_CEMENT
        ///// <summary>
        ///// 获取固井参数ID
        ///// </summary>
        ///// <returns></returns>
        //[WebMethod(EnableSession = true)]
        //public string GetCEMENTID()
        //{
        //    var num = DbHelperOra.GetSingle("select max(to_number(CEMENTID))  From PRO_LOG_CEMENT");
        //    return num == null ? "1" : (Convert.ToInt32(num.ToString()) + 1).ToString();
        //}
        [WebMethod(EnableSession = true)]
        public bool Savedata_固井参数(byte[] data_固井参数)
        {
            if (data_固井参数 == null) return false;
            Model.PRO_LOG_CEMENT model = Utility.ModelHelper.DeserializeObject(data_固井参数) as Model.PRO_LOG_CEMENT;
            if (model == null) return false;
            StringBuilder strSql = new StringBuilder();
            List<OracleParameter> parameters = new List<OracleParameter>();
            if (model.CEMENTID == null)
            {
                strSql.Append("insert into PRO_LOG_CEMENT(");
                strSql.Append("CEMENTID,JOB_PLAN_CD,REQUISITION_CD,UPDATE_DATE,CEMENT_PROPERTIES,CEMENT_DENSITY_MAX_VALUE,CEMENT_DENSITY_MIN_VALUE,CEMENT_TYPE,CEMENTED_QUANTITY,CASING_SHOE_DEPTH,CEMENT_PRE_TOP,CEMENT_HEIGHT,CEMENT_WELL_DATE,OPEN_WELL_DATE,DISTANCE_TUBING_AND_BUSHING,CASING_TOP_SPACING)");
                strSql.Append(" values (");
                strSql.Append("CEMENTID_SEQ.nextval,:JOB_PLAN_CD,:REQUISITION_CD,:UPDATE_DATE,:CEMENT_PROPERTIES,:CEMENT_DENSITY_MAX_VALUE,:CEMENT_DENSITY_MIN_VALUE,:CEMENT_TYPE,:CEMENTED_QUANTITY,:CASING_SHOE_DEPTH,:CEMENT_PRE_TOP,:CEMENT_HEIGHT,:CEMENT_WELL_DATE,:OPEN_WELL_DATE,:DISTANCE_TUBING_AND_BUSHING,:CASING_TOP_SPACING)");

                //parameters.Add(ServiceUtils.CreateOracleParameter(":CEMENTID", OracleDbType.Varchar2, GetCEMENTID()));
                parameters.Add(ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, model.JOB_PLAN_CD));
                parameters.Add(ServiceUtils.CreateOracleParameter(":REQUISITION_CD", OracleDbType.NVarchar2, model.REQUISITION_CD));
            }
            else
            {
                strSql.Append("update PRO_LOG_CEMENT set ");
                //strSql.Append("JOB_PLAN_CD=:JOB_PLAN_CD,");
                //strSql.Append("REQUISITION_CD=:REQUISITION_CD,");
                strSql.Append("UPDATE_DATE=:UPDATE_DATE,");
                strSql.Append("CEMENT_PROPERTIES=:CEMENT_PROPERTIES,");
                strSql.Append("CEMENT_DENSITY_MAX_VALUE=:CEMENT_DENSITY_MAX_VALUE,");
                strSql.Append("CEMENT_DENSITY_MIN_VALUE=:CEMENT_DENSITY_MIN_VALUE,");
                strSql.Append("CEMENT_TYPE=:CEMENT_TYPE,");
                strSql.Append("CEMENTED_QUANTITY=:CEMENTED_QUANTITY,");
                strSql.Append("CASING_SHOE_DEPTH=:CASING_SHOE_DEPTH,");
                strSql.Append("CEMENT_PRE_TOP=:CEMENT_PRE_TOP,");
                strSql.Append("CEMENT_HEIGHT=:CEMENT_HEIGHT,");
                strSql.Append("CEMENT_WELL_DATE=:CEMENT_WELL_DATE,");
                strSql.Append("OPEN_WELL_DATE=:OPEN_WELL_DATE,");
                strSql.Append("DISTANCE_TUBING_AND_BUSHING=:DISTANCE_TUBING_AND_BUSHING,");
                strSql.Append("CASING_TOP_SPACING=:CASING_TOP_SPACING");
                strSql.Append(" where JOB_PLAN_CD=:JOB_PLAN_CD and CEMENTID=:CEMENTID ");
                parameters.Add(ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, model.JOB_PLAN_CD));
                parameters.Add(ServiceUtils.CreateOracleParameter(":CEMENTID", OracleDbType.Varchar2, model.CEMENTID));
            }
            parameters.AddRange(new OracleParameter[] {          
                        ServiceUtils.CreateOracleParameter(":UPDATE_DATE", OracleDbType.Date, model.UPDATE_DATE) ,            
                        ServiceUtils.CreateOracleParameter(":CEMENT_PROPERTIES", OracleDbType.Varchar2, model.CEMENT_PROPERTIES) ,            
                        ServiceUtils.CreateOracleParameter(":CEMENT_DENSITY_MAX_VALUE", OracleDbType.Decimal, model.CEMENT_DENSITY_MAX_VALUE) ,       
                        ServiceUtils.CreateOracleParameter(":CEMENT_DENSITY_MIN_VALUE", OracleDbType.Decimal, model.CEMENT_DENSITY_MIN_VALUE) , 
                        ServiceUtils.CreateOracleParameter(":CEMENT_TYPE", OracleDbType.Varchar2, model.CEMENT_TYPE) , 
                        ServiceUtils.CreateOracleParameter(":CEMENTED_QUANTITY", OracleDbType.Varchar2, model.CEMENTED_QUANTITY) ,            
                        ServiceUtils.CreateOracleParameter(":CASING_SHOE_DEPTH", OracleDbType.Decimal, model.CASING_SHOE_DEPTH),
                        ServiceUtils.CreateOracleParameter(":CEMENT_PRE_TOP", OracleDbType.Decimal, model.CEMENT_PRE_TOP) ,   
                        ServiceUtils.CreateOracleParameter(":CEMENT_HEIGHT", OracleDbType.Decimal, model.CEMENT_HEIGHT) ,            
                        ServiceUtils.CreateOracleParameter(":CEMENT_WELL_DATE", OracleDbType.Date, model.CEMENT_WELL_DATE) ,            
                        ServiceUtils.CreateOracleParameter(":OPEN_WELL_DATE", OracleDbType.Date, model.OPEN_WELL_DATE) ,            
                        ServiceUtils.CreateOracleParameter(":DISTANCE_TUBING_AND_BUSHING", OracleDbType.Decimal, model.DISTANCE_TUBING_AND_BUSHING) , 
                        ServiceUtils.CreateOracleParameter(":CASING_TOP_SPACING", OracleDbType.Decimal, model.CASING_TOP_SPACING)

                         });
            if (DbHelperOra.ExecuteSqlTran(strSql.ToString(), parameters.ToArray()) > 0) return true;
            return false;
        }
        /// <summary>
        /// 获取固井参数列表
        /// </summary>
        /// <param name="string_计划任务书编号"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public DataSet GetDataGList_固井参数(string string_计划任务书编号)
        {
            if (string.IsNullOrWhiteSpace(string_计划任务书编号)) return null;
            return DbHelperOra.Query("select * from PRO_LOG_CEMENT where JOB_PLAN_CD=:JOB_PLAN_CD", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, string_计划任务书编号) });
        }
        /// <summary>
        /// 获取固井参数列表
        /// </summary>
        /// <param name="string_计划任务书编号"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public DataSet GetData_固井参数(string CEMENTID)
        {
            if (string.IsNullOrWhiteSpace(CEMENTID)) return null;
            return DbHelperOra.Query("select * from PRO_LOG_CEMENT where CEMENTID=:CEMENTID", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":CEMENTID", OracleDbType.Varchar2, CEMENTID) });
        }
        #endregion
        #region 录井解释成果DM_LOG_LOGGING_INTERPRETATION
        ///// <summary>
        ///// 获取录井解释成果INTERPRETATION_CD
        ///// </summary>
        ///// <returns></returns>
        //[WebMethod(EnableSession = true)]
        //public string GetINTERPRETATION_CD()
        //{
        //    var num = DbHelperOra.GetSingle("select max(to_number(INTERPRETATION_CD))  From DM_LOG_LOGGING_INTERPRETATION");
        //    return num == null ? "1" : (Convert.ToInt32(num.ToString()) + 1).ToString();
        //}
        [WebMethod(EnableSession = true)]
        public bool Savedata_录井解释成果(byte[] data_录井解释成果)
        {
            if (data_录井解释成果 == null) return false;
            Model.DM_LOG_LOGGING_INTERPRETATION model = Utility.ModelHelper.DeserializeObject(data_录井解释成果) as Model.DM_LOG_LOGGING_INTERPRETATION;
            if (model == null) return false;
            StringBuilder strSql = new StringBuilder();
            List<OracleParameter> parameters = new List<OracleParameter>();
            if (model.INTERPRETATION_CD == null)
            {
                strSql.Append("insert into DM_LOG_LOGGING_INTERPRETATION (");
                strSql.Append("INTERPRETATION_CD,JOB_PLAN_CD,REQUISITION_CD,UPDATE_DATE,HORIZON,WELL_TOP_DEPTH,WELL_BOTTOM_DEPTH,THICKNESS,YSMC,YSMS,YGYS,YGJB,DLYGFXZ,DLYGJB,DISPLAY_TYPE,DISPLAY_GENERAY,DRILLING_CHANGE,HYBROCABON_VALUE,IGNITION,CL_ION_CONTENT)");
                strSql.Append(" values (");
                strSql.Append("INTERPRETATION_CD_SEQ.nextval,:JOB_PLAN_CD,:REQUISITION_CD,:UPDATE_DATE,:HORIZON,:WELL_TOP_DEPTH,:WELL_BOTTOM_DEPTH,:THICKNESS,:YSMC,:YSMS,:YGYS,:YGJB,:DLYGFXZ,:DLYGJB,:DISPLAY_TYPE,:DISPLAY_GENERAY,:DRILLING_CHANGE,:HYBROCABON_VALUE,:IGNITION,:CL_ION_CONTENT)");

                //parameters.Add(ServiceUtils.CreateOracleParameter(":INTERPRETATION_CD", OracleDbType.Varchar2, GetINTERPRETATION_CD()));
                parameters.Add(ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, model.JOB_PLAN_CD));
                parameters.Add(ServiceUtils.CreateOracleParameter(":REQUISITION_CD", OracleDbType.NVarchar2, model.REQUISITION_CD));
            }
            else
            {
                strSql.Append("update DM_LOG_LOGGING_INTERPRETATION set ");
                //strSql.Append("JOB_PLAN_CD=:JOB_PLAN_CD,");
                //strSql.Append("REQUISITION_CD=:REQUISITION_CD,");
                strSql.Append("UPDATE_DATE=:UPDATE_DATE,");
                strSql.Append("HORIZON=:HORIZON,");
                strSql.Append("WELL_TOP_DEPTH=:WELL_TOP_DEPTH,");
                strSql.Append("WELL_BOTTOM_DEPTH=:WELL_BOTTOM_DEPTH,");
                strSql.Append("THICKNESS=:THICKNESS,");
                strSql.Append("YSMC=:YSMC,");
                strSql.Append("YSMS=:YSMS,");
                strSql.Append("YGYS=:YGYS,");
                strSql.Append("YGJB=:YGJB,");
                strSql.Append("DLYGFXZ=:DLYGFXZ,");
                strSql.Append("DLYGJB=:DLYGJB,");
                strSql.Append("DISPLAY_TYPE=:DISPLAY_TYPE,");
                strSql.Append("DISPLAY_GENERAY=:DISPLAY_GENERAY,");
                strSql.Append("DRILLING_CHANGE=:DRILLING_CHANGE,");
                strSql.Append("HYBROCABON_VALUE=:HYBROCABON_VALUE,");
                strSql.Append("IGNITION=:IGNITION,");
                strSql.Append("CL_ION_CONTENT=:CL_ION_CONTENT");
                strSql.Append(" where JOB_PLAN_CD=:JOB_PLAN_CD and INTERPRETATION_CD=:INTERPRETATION_CD ");
                parameters.Add(ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, model.JOB_PLAN_CD));
                parameters.Add(ServiceUtils.CreateOracleParameter(":INTERPRETATION_CD", OracleDbType.Varchar2, model.INTERPRETATION_CD));
            }
            parameters.AddRange(new OracleParameter[] {          
                        ServiceUtils.CreateOracleParameter(":UPDATE_DATE", OracleDbType.Date, model.UPDATE_DATE) ,            
                        ServiceUtils.CreateOracleParameter(":HORIZON", OracleDbType.Char, model.HORIZON) ,            
                        ServiceUtils.CreateOracleParameter(":WELL_TOP_DEPTH", OracleDbType.Decimal, model.WELL_TOP_DEPTH) ,            
                        ServiceUtils.CreateOracleParameter(":WELL_BOTTOM_DEPTH", OracleDbType.Decimal, model.WELL_BOTTOM_DEPTH) ,            
                        ServiceUtils.CreateOracleParameter(":THICKNESS", OracleDbType.Decimal, model.THICKNESS),
                        ServiceUtils.CreateOracleParameter(":YSMC", OracleDbType.NVarchar2, model.YSMC) ,   
                        ServiceUtils.CreateOracleParameter(":YSMS", OracleDbType.NVarchar2, model.YSMS) ,            
                        ServiceUtils.CreateOracleParameter(":YGYS", OracleDbType.NVarchar2, model.YGYS) ,            
                        ServiceUtils.CreateOracleParameter(":YGJB", OracleDbType.Decimal, model.YGJB) ,            
                        ServiceUtils.CreateOracleParameter(":DLYGFXZ", OracleDbType.Decimal, model.DLYGFXZ) , 
                        ServiceUtils.CreateOracleParameter(":DLYGJB", OracleDbType.Char, model.DLYGJB), 
                        ServiceUtils.CreateOracleParameter(":DISPLAY_TYPE", OracleDbType.Varchar2, model.DISPLAY_TYPE), 
                        ServiceUtils.CreateOracleParameter(":DISPLAY_GENERAY", OracleDbType.NVarchar2, model.DISPLAY_GENERAY),
                        ServiceUtils.CreateOracleParameter(":DRILLING_CHANGE",OracleDbType.Varchar2,model.DRILLING_CHANGE),
                        ServiceUtils.CreateOracleParameter(":HYBROCABON_VALUE",OracleDbType.Decimal,model.HYBROCABON_VALUE),
                        ServiceUtils.CreateOracleParameter(":IGNITION",OracleDbType.Decimal,model.IGNITION),
                        ServiceUtils.CreateOracleParameter(":CL_ION_CONTENT",OracleDbType.Decimal,model.CL_ION_CONTENT)

                         });
            if (DbHelperOra.ExecuteSqlTran(strSql.ToString(), parameters.ToArray()) > 0) return true;
            return false;
        }
        /// <summary>
        /// 获取录井解释成果列表
        /// </summary>
        /// <param name="string_计划任务书编号"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public DataSet GetDataGList_录井解释成果(string string_计划任务书编号)
        {
            if (string.IsNullOrWhiteSpace(string_计划任务书编号)) return null;
            return DbHelperOra.Query("select * from DM_LOG_LOGGING_INTERPRETATION where JOB_PLAN_CD=:JOB_PLAN_CD ORDER BY WELL_TOP_DEPTH", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, string_计划任务书编号) });
        }
        /// <summary>
        /// 获取录井解释成果列表
        /// </summary>
        /// <param name="string_计划任务书编号"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public DataSet GetData_录井解释成果(string INTERPRETATION_CD)
        {
            if (string.IsNullOrWhiteSpace(INTERPRETATION_CD)) return null;
            return DbHelperOra.Query("select * from DM_LOG_LOGGING_INTERPRETATION where INTERPRETATION_CD=:INTERPRETATION_CD", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":INTERPRETATION_CD", OracleDbType.Varchar2, INTERPRETATION_CD) }); ;
        }
        #endregion
        #region 地层分层数据2COM_BASE_STRATA_LAYER2
        ///// <summary>
        ///// 获取地层分层数据序号
        ///// </summary>
        ///// <returns></returns>
        //[WebMethod(EnableSession = true)]
        //public int GetSEQ_NO()
        //{
        //    var num = DbHelperOra.GetSingle("select max(SEQ_NO)  From COM_BASE_STRATA_LAYER2");
        //    return num == null ? 1 : (Convert.ToInt32(num.ToString()) + 1);
        //}
        [WebMethod(EnableSession = true)]
        public bool Import_地层分层数据2(string job_plan_cd, string requistion_cd, DataTable data)
        {
            return FileImport.ImprotLayer(job_plan_cd, requistion_cd, data);
        }

        [WebMethod(EnableSession = true)]
        public bool Import_录井解释成果(string job_plan_cd, string requistion_cd, DataTable data)
        {
            return FileImport.ImportLoggingInterpretation(job_plan_cd, requistion_cd, data);
        }


        [WebMethod(EnableSession = true)]
        public bool Savedata_地层分层数据2(byte[] data_地层分层数据2)
        {
            if (data_地层分层数据2 == null) return false;
            Model.COM_BASE_STRATA_LAYER2 model = Utility.ModelHelper.DeserializeObject(data_地层分层数据2) as Model.COM_BASE_STRATA_LAYER2;
            if (model == null) return false;
            StringBuilder strSql = new StringBuilder();
            List<OracleParameter> parameters = new List<OracleParameter>();
            if (model.SEQ_NO == 0)
            {
                strSql.Append("insert into COM_BASE_STRATA_LAYER2(");
                strSql.Append("SEQ_NO,JOB_PLAN_CD,REQUISITION_CD,STRAT_UNIT_NAME,STRAT_UNIT_S_NAME,AGE_CODE,STRATUM_CODE,STRAT_RANK,PARENT_STRAT_RANK,ROCK_DESCRIBE,BOTTOM_DEPTH,VERTICAL_DEPTH,BOTTOM_HEIGHT,VERTICAL_THICKNESS,SLANT_THICNESS,RELATIONS,DATE_TYPE,P_SCHEME_DESC,ROW_STATE,CREATE_ORG,CREATE_USER,UPDATE_ORG,UPDATE_USER,CREATE_DATE,UPDATE_DATE,REMARK)");
                strSql.Append(" values (");
                strSql.Append("SEQ_NO_SEQ.nextval,:JOB_PLAN_CD,:REQUISITION_CD,:STRAT_UNIT_NAME,:STRAT_UNIT_S_NAME,:AGE_CODE,:STRATUM_CODE,:STRAT_RANK,:PARENT_STRAT_RANK,:ROCK_DESCRIBE,:BOTTOM_DEPTH,:VERTICAL_DEPTH,:BOTTOM_HEIGHT,:VERTICAL_THICKNESS,:SLANT_THICNESS,:RELATIONS,:DATE_TYPE,:P_SCHEME_DESC,:ROW_STATE,:CREATE_ORG,:CREATE_USER,:UPDATE_ORG,:UPDATE_USER,:CREATE_DATE,:UPDATE_DATE,:REMARK)");

                //parameters.Add(ServiceUtils.CreateOracleParameter(":SEQ_NO", OracleDbType.Int32, GetSEQ_NO()));
                parameters.Add(ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, model.JOB_PLAN_CD));
                parameters.Add(ServiceUtils.CreateOracleParameter(":REQUISITION_CD", OracleDbType.Varchar2, model.REQUISITION_CD));
            }
            else
            {
                strSql.Append("update COM_BASE_STRATA_LAYER2 set ");
                //strSql.Append("JOB_PLAN_CD=:JOB_PLAN_CD,");
                //strSql.Append("REQUISITION_CD=:REQUISITION_CD,");
                strSql.Append("STRAT_UNIT_NAME=:STRAT_UNIT_NAME,");
                strSql.Append("STRAT_UNIT_S_NAME=:STRAT_UNIT_S_NAME,");
                strSql.Append("AGE_CODE=:AGE_CODE,");
                strSql.Append("STRATUM_CODE=:STRATUM_CODE,");
                strSql.Append("STRAT_RANK=:STRAT_RANK,");
                strSql.Append("PARENT_STRAT_RANK=:PARENT_STRAT_RANK,");
                strSql.Append("ROCK_DESCRIBE=:ROCK_DESCRIBE,");
                strSql.Append("BOTTOM_DEPTH=:BOTTOM_DEPTH,");
                strSql.Append("VERTICAL_DEPTH=:VERTICAL_DEPTH,");
                strSql.Append("BOTTOM_HEIGHT=:BOTTOM_HEIGHT,");
                strSql.Append("VERTICAL_THICKNESS=:VERTICAL_THICKNESS,");
                strSql.Append("SLANT_THICNESS=:SLANT_THICNESS,");
                strSql.Append("RELATIONS=:RELATIONS,");
                strSql.Append("DATE_TYPE=:DATE_TYPE,");
                strSql.Append("P_SCHEME_DESC=:P_SCHEME_DESC,");
                strSql.Append("ROW_STATE=:ROW_STATE,");
                strSql.Append("CREATE_ORG=:CREATE_ORG,");
                strSql.Append("CREATE_USER=:CREATE_USER,");
                strSql.Append("UPDATE_ORG=:UPDATE_ORG,");
                strSql.Append("UPDATE_USER=:UPDATE_USER,");
                strSql.Append("CREATE_DATE=:CREATE_DATE,");
                strSql.Append("UPDATE_DATE=:UPDATE_DATE,");
                strSql.Append("REMARK=:REMARK");
                strSql.Append(" where JOB_PLAN_CD=:JOB_PLAN_CD and SEQ_NO=:SEQ_NO ");
                parameters.Add(ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, model.JOB_PLAN_CD));
                parameters.Add(ServiceUtils.CreateOracleParameter(":SEQ_NO", OracleDbType.Int32, model.SEQ_NO));
            }
            parameters.AddRange(new OracleParameter[] {          
                        ServiceUtils.CreateOracleParameter(":STRAT_UNIT_NAME", OracleDbType.Varchar2, model.STRAT_UNIT_NAME) ,            
                        ServiceUtils.CreateOracleParameter(":STRAT_UNIT_S_NAME", OracleDbType.Varchar2, model.STRAT_UNIT_S_NAME) ,            
                        ServiceUtils.CreateOracleParameter(":AGE_CODE", OracleDbType.Varchar2, model.AGE_CODE) ,            
                        ServiceUtils.CreateOracleParameter(":STRATUM_CODE", OracleDbType.Varchar2, model.STRATUM_CODE) ,            
                        ServiceUtils.CreateOracleParameter(":STRAT_RANK", OracleDbType.Varchar2, model.STRAT_RANK),
                        ServiceUtils.CreateOracleParameter(":PARENT_STRAT_RANK", OracleDbType.Varchar2, model.PARENT_STRAT_RANK) ,            
                        ServiceUtils.CreateOracleParameter(":ROCK_DESCRIBE", OracleDbType.Varchar2, model.ROCK_DESCRIBE),
                         ServiceUtils.CreateOracleParameter(":BOTTOM_DEPTH", OracleDbType.Decimal, model.BOTTOM_DEPTH) ,            
                        ServiceUtils.CreateOracleParameter(":VERTICAL_DEPTH", OracleDbType.Decimal, model.VERTICAL_DEPTH) ,            
                        ServiceUtils.CreateOracleParameter(":BOTTOM_HEIGHT", OracleDbType.Decimal, model.BOTTOM_HEIGHT) ,            
                        ServiceUtils.CreateOracleParameter(":VERTICAL_THICKNESS", OracleDbType.Decimal, model.VERTICAL_THICKNESS) ,            
                        ServiceUtils.CreateOracleParameter(":SLANT_THICNESS", OracleDbType.Decimal, model.SLANT_THICNESS),
                        ServiceUtils.CreateOracleParameter(":RELATIONS", OracleDbType.Varchar2, model.RELATIONS) ,            
                        ServiceUtils.CreateOracleParameter(":DATE_TYPE", OracleDbType.Varchar2, model.DATE_TYPE),
                         ServiceUtils.CreateOracleParameter(":P_SCHEME_DESC", OracleDbType.Varchar2, model.P_SCHEME_DESC) ,            
                        ServiceUtils.CreateOracleParameter(":ROW_STATE", OracleDbType.Varchar2, model.ROW_STATE) ,            
                        ServiceUtils.CreateOracleParameter(":CREATE_ORG", OracleDbType.Varchar2, model.CREATE_ORG) ,            
                        ServiceUtils.CreateOracleParameter(":CREATE_USER", OracleDbType.Varchar2, model.CREATE_USER) ,            
                        ServiceUtils.CreateOracleParameter(":UPDATE_ORG", OracleDbType.Varchar2, model.UPDATE_ORG),
                        ServiceUtils.CreateOracleParameter(":UPDATE_USER", OracleDbType.Varchar2, model.UPDATE_USER) , 
                        ServiceUtils.CreateOracleParameter(":CREATE_DATE", OracleDbType.Date, model.CREATE_DATE),
                        ServiceUtils.CreateOracleParameter(":UPDATE_DATE", OracleDbType.Date, model.UPDATE_DATE) , 
                        ServiceUtils.CreateOracleParameter(":REMARK", OracleDbType.Varchar2, model.REMARK)

                         });
            if (DbHelperOra.ExecuteSqlTran(strSql.ToString(), parameters.ToArray()) > 0) return true;
            return false;
        }
        /// <summary>
        /// 获取地层分层数据列表
        /// </summary>
        /// <param name="string_计划任务书编号"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public DataSet GetDataGList_地层分层数据2(string string_计划任务书编号)
        {
            if (string.IsNullOrWhiteSpace(string_计划任务书编号)) return null;
            return DbHelperOra.Query("select * from COM_BASE_STRATA_LAYER2 where JOB_PLAN_CD=:JOB_PLAN_CD ORDER BY BOTTOM_DEPTH", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, string_计划任务书编号) });
        }
        /// <summary>
        /// 获取地层分层数据列表
        /// </summary>
        /// <param name="string_计划任务书编号"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public DataSet GetData_地层分层数据(int SEQ_NO)
        {
            //if (string.IsNullOrWhiteSpace(SEQ_NO)) return null;
            return DbHelperOra.Query("select * from COM_BASE_STRATA_LAYER2 where SEQ_NO=:SEQ_NO", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":SEQ_NO", OracleDbType.Int32, SEQ_NO) });
        }
        #endregion
    }
}