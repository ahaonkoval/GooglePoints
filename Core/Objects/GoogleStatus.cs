﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Objects
{
    /*
        "OK" – ошибок нет, адрес обработан и получен хотя бы один геокод.
        "ZERO_RESULTS" – геокодирование успешно выполнено, однако результаты не найдены. Это может произойти, если геокодировщику был передан несуществующий адрес (address).
        "OVER_QUERY_LIMIT" – указывает на превышение квоты.
        "REQUEST_DENIED" – указывает на отклонение запроса.
        "INVALID_REQUEST" – как правило, указывает на отсутствие в запросе полей address, components или latlng.
        "UNKNOWN_ERROR" – указывает, что запрос не удалось обработать из-за ошибки сервера. Если повторить попытку, запрос может оказаться успешным.
     */
    public enum GoogleStatus {
        OK = 1,
        ZERO_RESULTS = 2,
        OVER_QUERY_LIMIT = 3,
        REQUEST_DENIED = 4,
        INVALID_REQUEST = 5,
        UNKNOWN_ERROR = 6,
        MORE_ONE_POINT = 7
    };
}