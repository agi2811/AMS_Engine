namespace AMS_Engine
{
    class classQuery
    {
        public static string strQueryCodeInstruction = "\r\n                SELECT master_instruction.description\r\n                FROM (master_transmitter_head\r\n                   INNER JOIN ((master_zone_head\r\n                      INNER JOIN master_zone_detail\r\n                         ON master_zone_head.id = master_zone_detail.id)\r\n                         INNER JOIN ((master_line\r\n                            INNER JOIN master_plant\r\n                               ON master_line.plant_id = master_plant.id)\r\n                               INNER JOIN master_operation\r\n                                  ON master_line.operation_id = master_operation.id)\r\n                            ON master_zone_head.line_id = master_line.id)\r\n                      ON master_transmitter_head.zone_id = master_zone_head.id AND master_transmitter_head.zone_code = master_zone_detail.code)\r\n                   INNER JOIN master_transmitter_detail\r\n                      ON master_transmitter_head.id = master_transmitter_detail.id\r\n                   INNER JOIN (master_type_head\r\n                      INNER JOIN (master_type_detail\r\n                         INNER JOIN master_instruction\r\n                            ON master_type_detail.instruction_id = master_instruction.id)\r\n                         ON master_type_head.id = master_type_detail.id)\r\n                      ON master_transmitter_detail.type_id = master_type_head.id ";
        public static string strQueryTask = "\r\n                SELECT DISTINCT master_instruction.task\r\n                FROM (master_transmitter_head\r\n                   INNER JOIN ((master_zone_head\r\n                      INNER JOIN master_zone_detail\r\n                         ON master_zone_head.id = master_zone_detail.id)\r\n                         INNER JOIN ((master_line\r\n                            INNER JOIN master_plant\r\n                               ON master_line.plant_id = master_plant.id)\r\n                               INNER JOIN master_operation\r\n                                  ON master_line.operation_id = master_operation.id)\r\n                            ON master_zone_head.line_id = master_line.id)\r\n                      ON master_transmitter_head.zone_id = master_zone_head.id)\r\n                   INNER JOIN master_transmitter_detail\r\n                      ON master_transmitter_head.id = master_transmitter_detail.id\r\n                   INNER JOIN (master_type_head\r\n                      INNER JOIN (master_type_detail\r\n                         INNER JOIN master_instruction\r\n                            ON master_type_detail.instruction_id = master_instruction.id)\r\n                         ON master_type_head.id = master_type_detail.id)\r\n                      ON master_transmitter_detail.type_id = master_type_head.id ";
        //public static string strQueryCallHead = "\r\n                SELECT CASE WHEN andon_call_head.id is null THEN '0' ELSE '1' END AS call_flag\r\n                     , andon_call_head.id AS id_head\r\n                     , master_instruction.task AS task\r\n                FROM (master_transmitter_head\r\n                   INNER JOIN master_transmitter_detail\r\n                      ON master_transmitter_head.id = master_transmitter_detail.id\r\n                      INNER JOIN (master_type_head\r\n                         INNER JOIN (master_type_detail\r\n                            INNER JOIN master_instruction\r\n                               ON master_type_detail.instruction_id = master_instruction.id)\r\n                            ON master_type_head.id = master_type_detail.id)\r\n                         ON master_transmitter_detail.type_id = master_type_head.id)\r\n                   LEFT JOIN andon_call_head\r\n                      ON master_transmitter_head.id = andon_call_head.transmitter_id\r\n                      AND master_transmitter_detail.code = andon_call_head.transmitter_code\r\n                      AND master_type_detail.button_number = andon_call_head.button_number ";
        //public static string strQueryButtonId = "\r\n                SELECT master_transmitter_head.id\r\n                     , CASE WHEN master_instruction.task = 'Calling' THEN '1' ELSE '5' END task\r\n                FROM master_transmitter_head\r\n                   INNER JOIN master_transmitter_detail\r\n                      ON master_transmitter_head.id = master_transmitter_detail.id\r\n                      INNER JOIN (master_type_head\r\n                         INNER JOIN (master_type_detail\r\n                            INNER JOIN master_instruction\r\n                               ON master_type_detail.instruction_id = master_instruction.id)\r\n                            ON master_type_head.id = master_type_detail.id)\r\n                         ON master_transmitter_detail.type_id = master_type_head.id ";
        //public static string strQueryDataButton = "\r\n                  SELECT andon_call_head.id call_id\r\n                       , master_instruction.task\r\n                       , master_instruction.description\r\n                       , master_plant.description AS plant\r\n                       , master_operation.code AS operation\r\n                       , master_line.code AS line\r\n                       , CONCAT(master_zone_detail.code, master_transmitter_detail.description) AS zonenum\r\n                       , master_instruction.url\r\n                       , andon_call_head.button_number\r\n                       , andon_call_head.create_date\r\n                       , master_sound.file_path AS file_path\r\n , master_zone_detail.code                  FROM andon_call_head\r\n                     INNER JOIN (((master_transmitter_head\r\n                        INNER JOIN ((master_zone_head\r\n                           INNER JOIN master_zone_detail\r\n                              ON master_zone_head.id = master_zone_detail.id)\r\n                              INNER JOIN ((master_line\r\n                                 INNER JOIN master_plant\r\n                                    ON master_line.plant_id = master_plant.id)\r\n                                    INNER JOIN master_operation\r\n                                       ON master_line.operation_id = master_operation.id)\r\n                                 ON master_zone_head.line_id = master_line.id)\r\n                           ON master_transmitter_head.zone_id = master_zone_head.id)\r\n                        INNER JOIN master_transmitter_detail\r\n                           ON master_transmitter_head.id = master_transmitter_detail.id)\r\n                        INNER JOIN (master_type_head\r\n                           INNER JOIN ((master_type_detail\r\n                              INNER JOIN master_instruction\r\n                                 ON master_type_detail.instruction_id = master_instruction.id)\r\n                                 LEFT JOIN master_sound\r\n                                    ON master_type_detail.sound_id = master_sound.id)\r\n                              ON master_type_head.id = master_type_detail.id)\r\n                           ON master_transmitter_detail.type_id = master_type_head.id)\r\n                        ON andon_call_head.transmitter_id = master_transmitter_head.id\r\n                        AND andon_call_head.transmitter_code = master_transmitter_detail.code\r\n                        AND andon_call_head.button_number = master_type_detail.button_number ";
        //public static string strQueryDataStop = "\r\n                  SELECT 0 AS call_id\r\n                       , master_instruction.task\r\n                       , master_instruction.description\r\n                       , master_plant.description AS plant\r\n                       , master_operation.code AS operation\r\n                       , master_line.code AS line\r\n                       , CONCAT(master_zone_detail.code, master_transmitter_detail.description)AS zone\r\n                       , master_instruction.url\r\n                       , '00' AS button_number\r\n                       , GETDATE()\r\n                       , '-' AS files_path\r\n , master_zone_detail.code                 FROM (((master_transmitter_head\r\n                        INNER JOIN ((master_zone_head\r\n                           INNER JOIN master_zone_detail\r\n                              ON master_zone_head.id = master_zone_detail.id)\r\n                              INNER JOIN ((master_line\r\n                                 INNER JOIN master_plant\r\n                                    ON master_line.plant_id = master_plant.id)\r\n                                    INNER JOIN master_operation\r\n                                       ON master_line.operation_id = master_operation.id)\r\n                                 ON master_zone_head.line_id = master_line.id)\r\n                           ON master_transmitter_head.zone_id = master_zone_head.id)\r\n                        INNER JOIN master_transmitter_detail\r\n                           ON master_transmitter_head.id = master_transmitter_detail.id)\r\n                        INNER JOIN (master_type_head\r\n                           INNER JOIN (master_type_detail\r\n                              INNER JOIN master_instruction\r\n                                 ON master_type_detail.instruction_id = master_instruction.id)\r\n                              ON master_type_head.id = master_type_detail.id)\r\n                           ON master_transmitter_detail.type_id = master_type_head.id)\r\n                        LEFT JOIN (SELECT transmitter_id, transmitter_code, COUNT(id) AS countCall FROM andon_call_head WHERE status <> '5'\r\n                           GROUP BY transmitter_id, transmitter_code) sqlCount\r\n                           ON master_transmitter_head.id = sqlCount.transmitter_id\r\n                           AND master_transmitter_detail.code = sqlCount.transmitter_code ";
        //public static string strQueryDataCallLine1 = "\r\n                  SELECT view_light.code AS line, COUNT(view_light.id) AS count_call FROM (SELECT master_light_detail.code, andon_call_head.id FROM andon_call_head\r\n                     INNER JOIN ((((master_transmitter_head\r\n                        INNER JOIN ((master_zone_head\r\n                           INNER JOIN master_zone_detail\r\n                              ON master_zone_head.id = master_zone_detail.id)\r\n                              INNER JOIN ((master_line\r\n                                 INNER JOIN master_plant\r\n                                    ON master_line.plant_id = master_plant.id)\r\n                                    INNER JOIN master_operation\r\n                                       ON master_line.operation_id = master_operation.id)\r\n                                 ON master_zone_head.line_id = master_line.id)\r\n                           ON master_transmitter_head.zone_id = master_zone_head.id)\r\n                        INNER JOIN master_transmitter_detail\r\n                           ON master_transmitter_head.id = master_transmitter_detail.id)\r\n                        INNER JOIN (master_type_head\r\n                           INNER JOIN ((master_type_detail\r\n                              INNER JOIN master_instruction\r\n                                 ON master_type_detail.instruction_id = master_instruction.id)\r\n                                 LEFT JOIN master_sound\r\n                                    ON master_type_detail.sound_id = master_sound.id)\r\n                              ON master_type_head.id = master_type_detail.id)\r\n                           ON master_transmitter_detail.type_id = master_type_head.id)\r\n                        INNER JOIN (master_light_head\r\n                           INNER JOIN master_light_detail\r\n                              ON master_light_head.id = master_light_detail.id)\r\n                           ON master_line.id = master_light_head.line_id)\r\n                        ON andon_call_head.transmitter_id = master_transmitter_head.id\r\n                        AND andon_call_head.transmitter_code = master_transmitter_detail.code\r\n                        AND andon_call_head.button_number = master_type_detail.button_number ";
        //public static string strQueryDataCallLine2 = "\r\n                  SELECT distinct master_light_detail.code AS line\r\n                       , 1 AS count_call\r\n                  FROM ((((master_transmitter_head\r\n                        INNER JOIN ((master_zone_head\r\n                           INNER JOIN master_zone_detail\r\n                              ON master_zone_head.id = master_zone_detail.id)\r\n                              INNER JOIN ((master_line\r\n                                 INNER JOIN master_plant\r\n                                    ON master_line.plant_id = master_plant.id)\r\n                                    INNER JOIN master_operation\r\n                                       ON master_line.operation_id = master_operation.id)\r\n                                 ON master_zone_head.line_id = master_line.id)\r\n                           ON master_transmitter_head.zone_id = master_zone_head.id)\r\n                        INNER JOIN master_transmitter_detail\r\n                           ON master_transmitter_head.id = master_transmitter_detail.id)\r\n                        INNER JOIN (master_type_head\r\n                           INNER JOIN ((master_type_detail\r\n                              INNER JOIN master_instruction\r\n                                 ON master_type_detail.instruction_id = master_instruction.id)\r\n                                 LEFT JOIN master_sound\r\n                                    ON master_type_detail.sound_id = master_sound.id)\r\n                              ON master_type_head.id = master_type_detail.id)\r\n                           ON master_transmitter_detail.type_id = master_type_head.id)\r\n                        INNER JOIN (master_light_head\r\n                           INNER JOIN master_light_detail\r\n                              ON master_light_head.id = master_light_detail.id)\r\n                           ON master_line.id = master_light_head.line_id) ";

        public static string strQueryCallHead = @"
                SELECT CASE WHEN andon_call_head.id is null THEN '0' ELSE '1' END AS call_flag
                       , andon_call_head.id AS id_head
                       , master_instruction.task AS task
                  FROM (((((master_transmitter_head
                     INNER JOIN master_transmitter_detail
                        ON master_transmitter_head.id = master_transmitter_detail.id
                        INNER JOIN (master_type_head
                           INNER JOIN (master_type_detail
                              INNER JOIN master_instruction
                                 ON master_type_detail.instruction_id = master_instruction.id)
                              ON master_type_head.id = master_type_detail.id)
                           ON master_transmitter_detail.type_id = master_type_head.id)
                     INNER JOIN ((master_zone_head
                        INNER JOIN master_zone_detail
                           ON master_zone_head.id = master_zone_detail.id)
					    INNER JOIN ((master_line
						   INNER JOIN master_plant
						      ON master_line.plant_id = master_plant.id)
						   INNER JOIN master_operation
							  ON master_line.operation_id = master_operation.id)
						   ON master_zone_head.line_id = master_line.id)
                        ON master_transmitter_head.zone_id = master_zone_head.id
                        AND master_transmitter_head.zone_code = master_zone_detail.code)
                     INNER JOIN (master_receiver_head
                        INNER JOIN master_receiver_detail
                           ON master_receiver_head.id = master_receiver_detail.id)
                        ON master_transmitter_head.receiver_id = master_receiver_head.id
                        AND master_transmitter_head.receiver_code = master_receiver_detail.code)
                     INNER JOIN master_monitor
                        ON master_transmitter_head.monitor_id = master_monitor.id)
                     LEFT JOIN andon_call_head
                        ON master_transmitter_head.receiver_id = andon_call_head.receiver_id
                        AND master_transmitter_head.receiver_code = andon_call_head.receiver_code
                        AND master_transmitter_detail.code = andon_call_head.transmitter_code
                        AND master_type_detail.button_number = andon_call_head.button_number) ";

        /*master_transmitter_head.zone_id = andon_call_head.zone_id
        AND master_transmitter_head.zone_code = andon_call_head.zone_code
        AND master_transmitter_head.monitor_id = andon_call_head.monitor_id*/

        public static string strQueryTransmitterData = @"
                 SELECT TOP 1 master_transmitter_head.zone_id
                     , master_transmitter_head.zone_code
                     , master_transmitter_head.receiver_id
                     , master_transmitter_head.receiver_code
                     , master_transmitter_head.monitor_id
                     , CASE WHEN master_person_detail.employee_id is null THEN '' ELSE master_person_detail.employee_id END AS employee_id
                     , CASE WHEN master_person_detail.employee_name is null THEN '' ELSE master_person_detail.employee_name END AS employee_name
                     , CASE WHEN master_person_detail.phone_number is null THEN '' ELSE master_person_detail.phone_number END AS phone_number
                     , master_transmitter_detail.register
                     , master_transmitter_detail.description
                     , master_transmitter_detail.type_id
                     , master_instruction.description AS 'instruction'
                     , CASE WHEN master_instruction.task = 'Calling' THEN '1' ELSE '5' END AS task
                FROM (((master_transmitter_head
                   INNER JOIN master_transmitter_detail
                      ON master_transmitter_head.id = master_transmitter_detail.id
                      INNER JOIN (master_type_head
                         INNER JOIN (master_type_detail
                            INNER JOIN master_instruction
                               ON master_type_detail.instruction_id = master_instruction.id)
                            ON master_type_head.id = master_type_detail.id)
                         ON master_transmitter_detail.type_id = master_type_head.id)
				   INNER JOIN ((master_zone_head
				      INNER JOIN master_zone_detail
					     ON master_zone_head.id = master_zone_detail.id)
					  INNER JOIN ((master_line
					     INNER JOIN master_plant
						    ON master_line.plant_id = master_plant.id)
						 INNER JOIN master_operation
						    ON master_line.operation_id = master_operation.id)
						 ON master_zone_head.line_id = master_line.id)
				      ON master_transmitter_head.zone_id = master_zone_head.id
				      AND master_transmitter_head.zone_code = master_zone_detail.code)
                   LEFT JOIN (master_person_head
                      INNER JOIN master_person_detail
                         ON master_person_head.id = master_person_detail.id
                         AND master_person_detail.status = '1')
                      ON master_transmitter_head.zone_id = master_transmitter_head.zone_id
                      AND master_transmitter_head.zone_code = master_transmitter_head.zone_code
					  AND master_zone_head.line_id = master_person_head.line_id
					  AND master_instruction.description = master_person_detail.job_desc) ";

        public static string strQueryDataCallLine1 = @"
                SELECT view_light.code AS line
                     , COUNT(view_light.id) AS count_call
                FROM (
                SELECT master_light_detail.code
                     , andon_call_head.id
                FROM ((andon_call_head
                   INNER JOIN ((master_zone_head
                      INNER JOIN master_zone_detail
                         ON master_zone_head.id = master_zone_detail.id)
                         INNER JOIN ((master_line
                            INNER JOIN master_plant
                               ON master_line.plant_id = master_plant.id)
                               INNER JOIN master_operation
                                  ON master_line.operation_id = master_operation.id)
                               ON master_zone_head.line_id = master_line.id)
                            ON andon_call_head.zone_id = master_zone_head.id
                            AND andon_call_head.zone_code = master_zone_detail.code)
                   INNER JOIN (master_light_head
                      INNER JOIN master_light_detail
                         ON master_light_head.id = master_light_detail.id)
                      ON master_zone_head.id = master_light_head.zone_id
                      AND master_zone_detail.code = master_light_head.zone_code) ";

        public static string strQueryDataCallLine2 = @"
                SELECT view_light.code AS line
                     , COUNT(view_light.code) AS count_call
                FROM 
                  (SELECT distinct master_light_detail.code
                       , 1 AS count_call
                  FROM ((((master_transmitter_head
                        INNER JOIN ((master_zone_head
                           INNER JOIN master_zone_detail
                              ON master_zone_head.id = master_zone_detail.id)
                              INNER JOIN ((master_line
                                 INNER JOIN master_plant
                                    ON master_line.plant_id = master_plant.id)
                                    INNER JOIN master_operation
                                       ON master_line.operation_id = master_operation.id)
                                 ON master_zone_head.line_id = master_line.id)
                           ON master_transmitter_head.zone_id = master_zone_head.id)
                        INNER JOIN master_transmitter_detail
                           ON master_transmitter_head.id = master_transmitter_detail.id)
                        INNER JOIN (master_type_head
                           INNER JOIN ((master_type_detail
                              INNER JOIN master_instruction
                                 ON master_type_detail.instruction_id = master_instruction.id)
                                 LEFT JOIN master_sound
                                    ON master_type_detail.sound_id = master_sound.id)
                              ON master_type_head.id = master_type_detail.id)
                           ON master_transmitter_detail.type_id = master_type_head.id)
                        INNER JOIN (master_light_head
                           INNER JOIN master_light_detail
                              ON master_light_head.id = master_light_detail.id)
                           ON master_zone_head.id = master_light_head.zone_id
                           AND master_zone_detail.code = master_light_head.zone_code) ";

        //public static string strQueryCodeInstruction = @"
        //        SELECT master_instruction.description
        //        FROM (master_transmitter_head
        //           INNER JOIN ((master_zone_head
        //              INNER JOIN master_zone_detail
        //                 ON master_zone_head.id = master_zone_detail.id)
        //                 INNER JOIN ((master_line
        //                    INNER JOIN master_plant
        //                       ON master_line.plant_id = master_plant.id)
        //                       INNER JOIN master_operation
        //                          ON master_line.operation_id = master_operation.id)
        //                    ON master_zone_head.line_id = master_line.id)
        //              ON master_transmitter_head.zone_id = master_zone_head.id)
        //           INNER JOIN master_transmitter_detail
        //              ON master_transmitter_head.id = master_transmitter_detail.id
        //           INNER JOIN (master_type_head
        //              INNER JOIN (master_type_detail
        //                 INNER JOIN master_instruction
        //                    ON master_type_detail.instruction_id = master_instruction.id)
        //                 ON master_type_head.id = master_type_detail.id)
        //              ON master_transmitter_detail.type_id = master_type_head.id ";

        //public static string strQueryTask = @"
        //        SELECT master_instruction.task
        //        FROM (master_transmitter_head
        //           INNER JOIN ((master_zone_head
        //              INNER JOIN master_zone_detail
        //                 ON master_zone_head.id = master_zone_detail.id)
        //                 INNER JOIN ((master_line
        //                    INNER JOIN master_plant
        //                       ON master_line.plant_id = master_plant.id)
        //                       INNER JOIN master_operation
        //                          ON master_line.operation_id = master_operation.id)
        //                    ON master_zone_head.line_id = master_line.id)
        //              ON master_transmitter_head.zone_id = master_zone_head.id)
        //           INNER JOIN master_transmitter_detail
        //              ON master_transmitter_head.id = master_transmitter_detail.id
        //           INNER JOIN (master_type_head
        //              INNER JOIN (master_type_detail
        //                 INNER JOIN master_instruction
        //                    ON master_type_detail.instruction_id = master_instruction.id)
        //                 ON master_type_head.id = master_type_detail.id)
        //              ON master_transmitter_detail.type_id = master_type_head.id ";

        public static string strQueryButtonNumber = @"
                SELECT master_type_detail.button_number
                FROM (master_type_head
                   INNER JOIN (master_type_detail
                      INNER JOIN master_instruction
                         ON master_type_detail.instruction_id = master_instruction.id)
                      ON master_type_head.id = master_type_detail.id) ";

        //public static string strQueryCallHead = @"
        //        SELECT CASE WHEN andon_call_head.id is null THEN '0' ELSE '1' END AS call_flag
        //             , andon_call_head.id AS id_head
        //        FROM ((master_transmitter_head
        //           INNER JOIN master_transmitter_detail
        //              ON master_transmitter_head.id = master_transmitter_detail.id
        //              INNER JOIN (master_type_head
        //                 INNER JOIN (master_type_detail
        //                    INNER JOIN master_instruction
        //                       ON master_type_detail.instruction_id = master_instruction.id)
        //                    ON master_type_head.id = master_type_detail.id)
        //                 ON master_transmitter_detail.type_id = master_type_head.id)
        //           LEFT JOIN andon_call_head
        //              ON master_transmitter_head.id = andon_call_head.transmitter_id
        //              AND master_transmitter_detail.code = andon_call_head.transmitter_code
        //              AND master_type_detail.button_number = andon_call_head.button_number) ";

        //public static string strQueryButtonId = @"
        //        SELECT master_transmitter_head.id
        //             , CASE WHEN master_instruction.task = 'Calling' THEN '1' ELSE '5' END task
        //        FROM (master_transmitter_head
        //           INNER JOIN master_transmitter_detail
        //              ON master_transmitter_head.id = master_transmitter_detail.id
        //              INNER JOIN (master_type_head
        //                 INNER JOIN (master_type_detail
        //                    INNER JOIN master_instruction
        //                       ON master_type_detail.instruction_id = master_instruction.id)
        //                    ON master_type_head.id = master_type_detail.id)
        //                 ON master_transmitter_detail.type_id = master_type_head.id) ";

        //public static string strQueryDataButton = @"
        //          SELECT andon_call_head.id call_id
        //               , master_instruction.task
        //               , master_instruction.description
        //               , master_plant.description AS plant
        //               , master_operation.code AS operation
        //               , master_line.code AS line
        //               , master_zone_detail.code AS zone
        //               , master_instruction.url
        //               , andon_call_head.button_number
        //               , andon_call_head.create_date
        //               , master_sound.file_path AS file_path
        //          FROM (andon_call_head
        //             INNER JOIN (((master_transmitter_head
        //                INNER JOIN ((master_zone_head
        //                   INNER JOIN master_zone_detail
        //                      ON master_zone_head.id = master_zone_detail.id)
        //                      INNER JOIN ((master_line
        //                         INNER JOIN master_plant
        //                            ON master_line.plant_id = master_plant.id)
        //                            INNER JOIN master_operation
        //                               ON master_line.operation_id = master_operation.id)
        //                         ON master_zone_head.line_id = master_line.id)
        //                   ON master_transmitter_head.zone_id = master_zone_head.id)
        //                INNER JOIN master_transmitter_detail
        //                   ON master_transmitter_head.id = master_transmitter_detail.id)
        //                INNER JOIN (master_type_head
        //                   INNER JOIN ((master_type_detail
        //                      INNER JOIN master_instruction
        //                         ON master_type_detail.instruction_id = master_instruction.id)
        //                         INNER JOIN master_sound
        //                            ON master_type_detail.sound_id = master_sound.id)
        //                      ON master_type_head.id = master_type_detail.id)
        //                   ON master_transmitter_detail.type_id = master_type_head.id)
        //                ON andon_call_head.transmitter_id = master_transmitter_head.id
        //                AND andon_call_head.transmitter_code = master_transmitter_detail.code
        //                AND andon_call_head.button_number = master_type_detail.button_number) ";

        //public static string strQueryDataStop = @"
        //          SELECT 0 AS call_id
        //               , master_instruction.task
        //               , master_instruction.description
        //               , master_plant.description AS plant
        //               , master_operation.code AS operation
        //               , master_line.code AS line
        //               , master_zone_detail.code AS zone
        //               , master_instruction.url
        //               , '00' AS button_number
        //               , GETDATE()
        //               , '-' AS files
        //          FROM ((((master_transmitter_head
        //                INNER JOIN ((master_zone_head
        //                   INNER JOIN master_zone_detail
        //                      ON master_zone_head.id = master_zone_detail.id)
        //                      INNER JOIN ((master_line
        //                         INNER JOIN master_plant
        //                            ON master_line.plant_id = master_plant.id)
        //                            INNER JOIN master_operation
        //                               ON master_line.operation_id = master_operation.id)
        //                         ON master_zone_head.line_id = master_line.id)
        //                   ON master_transmitter_head.zone_id = master_zone_head.id)
        //                INNER JOIN master_transmitter_detail
        //                   ON master_transmitter_head.id = master_transmitter_detail.id)
        //                INNER JOIN (master_type_head
        //                   INNER JOIN (master_type_detail
        //                      INNER JOIN master_instruction
        //                         ON master_type_detail.instruction_id = master_instruction.id)
        //                      ON master_type_head.id = master_type_detail.id)
        //                   ON master_transmitter_detail.type_id = master_type_head.id)
        //                LEFT JOIN (SELECT transmitter_id, transmitter_code, COUNT(id) AS countCall FROM andon_call_head WHERE status <> '5'
        //                   GROUP BY transmitter_id, transmitter_code) sqlCount
        //                   ON master_transmitter_head.id = sqlCount.transmitter_id
        //                   AND master_transmitter_detail.code = sqlCount.transmitter_code) ";

        //public static string strQueryDataCallLine1 = @"
        //          SELECT view_light.code AS line
        //               , COUNT(view_light.id) AS count_call
        //          FROM (
        //          SELECT master_light_detail.code
        //               , andon_call_head.id
        //          FROM (andon_call_head
        //             INNER JOIN ((((master_transmitter_head
        //                INNER JOIN ((master_zone_head
        //                   INNER JOIN master_zone_detail
        //                      ON master_zone_head.id = master_zone_detail.id)
        //                      INNER JOIN ((master_line
        //                         INNER JOIN master_plant
        //                            ON master_line.plant_id = master_plant.id)
        //                            INNER JOIN master_operation
        //                               ON master_line.operation_id = master_operation.id)
        //                         ON master_zone_head.line_id = master_line.id)
        //                   ON master_transmitter_head.zone_id = master_zone_head.id)
        //                INNER JOIN master_transmitter_detail
        //                   ON master_transmitter_head.id = master_transmitter_detail.id)
        //                INNER JOIN (master_type_head
        //                   INNER JOIN ((master_type_detail
        //                      INNER JOIN master_instruction
        //                         ON master_type_detail.instruction_id = master_instruction.id)
        //                         INNER JOIN master_sound
        //                            ON master_type_detail.sound_id = master_sound.id)
        //                      ON master_type_head.id = master_type_detail.id)
        //                   ON master_transmitter_detail.type_id = master_type_head.id)
        //                INNER JOIN (master_light_head
        //                   INNER JOIN master_light_detail
        //                      ON master_light_head.id = master_light_detail.id)
        //                   ON master_line.id = master_light_head.line_id)
        //                ON andon_call_head.transmitter_id = master_transmitter_head.id
        //                AND andon_call_head.transmitter_code = master_transmitter_detail.code
        //                AND andon_call_head.button_number = master_type_detail.button_number) ";

        //public static string strQueryDataCallLine2 = @"
        //          SELECT master_light_detail.code AS line
        //               , 1 AS count_call
        //          FROM ((((master_transmitter_head
        //                INNER JOIN ((master_zone_head
        //                   INNER JOIN master_zone_detail
        //                      ON master_zone_head.id = master_zone_detail.id)
        //                      INNER JOIN ((master_line
        //                         INNER JOIN master_plant
        //                            ON master_line.plant_id = master_plant.id)
        //                            INNER JOIN master_operation
        //                               ON master_line.operation_id = master_operation.id)
        //                         ON master_zone_head.line_id = master_line.id)
        //                   ON master_transmitter_head.zone_id = master_zone_head.id)
        //                INNER JOIN master_transmitter_detail
        //                   ON master_transmitter_head.id = master_transmitter_detail.id)
        //                INNER JOIN (master_type_head
        //                   INNER JOIN ((master_type_detail
        //                      INNER JOIN master_instruction
        //                         ON master_type_detail.instruction_id = master_instruction.id)
        //                         INNER JOIN master_sound
        //                            ON master_type_detail.sound_id = master_sound.id)
        //                      ON master_type_head.id = master_type_detail.id)
        //                   ON master_transmitter_detail.type_id = master_type_head.id)
        //                INNER JOIN (master_light_head
        //                   INNER JOIN master_light_detail
        //                      ON master_light_head.id = master_light_detail.id)
        //                   ON master_line.id = master_light_head.line_id) ";

        public static string strQueryDataCallActive = @"
                  SELECT andon_call_head.transmitter_code
                       , andon_call_head.receiver_code
                  FROM (((andon_call_head
                     INNER JOIN ((master_zone_head
                        INNER JOIN master_zone_detail
                           ON master_zone_head.id = master_zone_detail.id)
                           INNER JOIN ((master_line
                              INNER JOIN master_plant
                                 ON master_line.plant_id = master_plant.id)
                                 INNER JOIN master_operation
                                    ON master_line.operation_id = master_operation.id)
                              ON master_zone_head.line_id = master_line.id)
                        ON andon_call_head.zone_id = master_zone_head.id)
                     INNER JOIN (master_type_head
                        INNER JOIN ((master_type_detail
                           INNER JOIN master_instruction
                              ON master_type_detail.instruction_id = master_instruction.id)
                              LEFT JOIN master_sound
                                 ON master_type_detail.sound_id = master_sound.id)
                           ON master_type_head.id = master_type_detail.id)
                        ON andon_call_head.type_id = master_type_head.id
                        AND andon_call_head.button_number = master_type_detail.button_number)
                     LEFT JOIN (master_light_head
                        INNER JOIN master_light_detail
                           ON master_light_head.id = master_light_detail.id)
                        ON master_zone_head.id = master_light_head.zone_id
                        AND master_zone_detail.code = master_light_head.zone_code) ";

        public static string strQueryDataTransmitter = @"
                  SELECT andon_call_head.id AS call_id
                       , master_instruction.task
                       , master_instruction.description AS instruction
                       , master_plant.description AS plant
                       , master_operation.code AS operation
                       , master_line.code AS line
                       , CONCAT(master_zone_detail.code, andon_call_head.description) AS zonenum
                       , master_instruction.url
                       , andon_call_head.button_number
                       , andon_call_head.create_date
                       , master_sound.file_path 
                       , master_zone_detail.code
                       , andon_call_head.zone_id
                       , master_monitor.serial_id
                  FROM (((andon_call_head
                     INNER JOIN ((master_zone_head
                        INNER JOIN master_zone_detail
                           ON master_zone_head.id = master_zone_detail.id)
                           INNER JOIN ((master_line
                              INNER JOIN master_plant
                                 ON master_line.plant_id = master_plant.id)
                                 INNER JOIN master_operation
                                    ON master_line.operation_id = master_operation.id)
                              ON master_zone_head.line_id = master_line.id)
                        ON andon_call_head.zone_id = master_zone_head.id
                        AND andon_call_head.zone_code = master_zone_detail.code)
                     INNER JOIN (master_type_head
                        INNER JOIN ((master_type_detail
                           INNER JOIN master_instruction
                              ON master_type_detail.instruction_id = master_instruction.id)
                              LEFT JOIN master_sound
                                 ON master_type_detail.sound_id = master_sound.id)
                           ON master_type_head.id = master_type_detail.id)
                        ON andon_call_head.type_id = master_type_head.id
                        AND andon_call_head.button_number = master_type_detail.button_number)
                     INNER JOIN master_monitor
                        ON andon_call_head.monitor_id = master_monitor.id) ";

        public static string strQueryDataStop = @"
                SELECT 0 AS call_id
                     , master_instruction.task
                     , master_instruction.description
                     , master_plant.description AS plant
                     , master_operation.code AS operation
                     , master_line.code AS line
                     , CONCAT(master_zone_detail.code, master_transmitter_detail.description)AS zone
                     , master_instruction.url
                     , '00' AS button_number
                     , GETDATE()
                     , '-' AS files_path
                     , master_zone_detail.code
                     , master_transmitter_head.zone_id
                     , master_monitor.serial_id
                FROM (((((master_transmitter_head
                   INNER JOIN ((master_zone_head
                      INNER JOIN master_zone_detail
                         ON master_zone_head.id = master_zone_detail.id)
                         INNER JOIN ((master_line
                            INNER JOIN master_plant
                               ON master_line.plant_id = master_plant.id)
                               INNER JOIN master_operation
                                  ON master_line.operation_id = master_operation.id)
                            ON master_zone_head.line_id = master_line.id)
                      ON master_transmitter_head.zone_id = master_zone_head.id
                      AND master_transmitter_head.zone_code = master_zone_detail.code)
                   INNER JOIN master_transmitter_detail
                      ON master_transmitter_head.id = master_transmitter_detail.id)
                   INNER JOIN (master_type_head
                      INNER JOIN (master_type_detail
                         INNER JOIN master_instruction
                            ON master_type_detail.instruction_id = master_instruction.id)
                         ON master_type_head.id = master_type_detail.id)
                      ON master_transmitter_detail.type_id = master_type_head.id)
                   INNER JOIN master_monitor
                      ON master_transmitter_head.monitor_id = master_monitor.id)
                   LEFT JOIN (SELECT zone_id, zone_code, receiver_id, receiver_code, transmitter_code, COUNT(id) AS countCall FROM andon_call_head WHERE status <> '5'
                      GROUP BY zone_id, zone_code, receiver_id, receiver_code, transmitter_code) sqlCount
                      ON master_transmitter_detail.code = sqlCount.transmitter_code) ";

        /*master_zone_head.id = sqlCount.zone_id
        AND master_zone_detail.code = sqlCount.zone_code
        AND master_zone_detail.code = sqlCount.zone_code
        AND master_zone_detail.code = sqlCount.zone_code
        AND */

        public static string strQueryDataPerson = @"
                SELECT master_person_detail.employee_id
                       , master_person_detail.employee_name
                       , master_person_detail.phone_number
                  FROM master_person_head
                     INNER JOIN master_person_detail
                        ON master_person_head.id = master_person_detail.id
					 INNER JOIN ((master_line
					    INNER JOIN master_plant
						   ON master_line.plant_id = master_plant.id)
					    INNER JOIN master_operation
					       ON master_line.operation_id = master_operation.id)
					    ON master_person_head.line_id = master_line.id ";
    }
}
