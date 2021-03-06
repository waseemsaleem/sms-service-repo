PGDMP     "            	        z         	   accountdb    14.2    14.2     ?           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false            ?           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false            ?           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false            ?           1262    16398 	   accountdb    DATABASE     j   CREATE DATABASE "accountdb" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE = 'English_Pakistan.1252';
    DROP DATABASE "accountdb";
                postgres    false            ?            1259    24588    account    TABLE     ?   CREATE TABLE "public"."account" (
    "id" integer NOT NULL,
    "auth_id" character varying(40),
    "username" character varying(30)
);
    DROP TABLE "public"."account";
       public         heap    postgres    false            ?            1259    24591    account_id_seq    SEQUENCE     {   CREATE SEQUENCE "public"."account_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 )   DROP SEQUENCE "public"."account_id_seq";
       public          postgres    false    209            ?           0    0    account_id_seq    SEQUENCE OWNED BY     K   ALTER SEQUENCE "public"."account_id_seq" OWNED BY "public"."account"."id";
          public          postgres    false    210            ?            1259    24592    phone_number    TABLE     ?   CREATE TABLE "public"."phone_number" (
    "id" integer NOT NULL,
    "number" character varying(40),
    "account_id" integer,
    "Text" character varying(400),
    "To" character varying(50),
    "From" character varying(50)
);
 $   DROP TABLE "public"."phone_number";
       public         heap    postgres    false            ?            1259    24595    phone_number_id_seq    SEQUENCE     ?   CREATE SEQUENCE "public"."phone_number_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 .   DROP SEQUENCE "public"."phone_number_id_seq";
       public          postgres    false    211            ?           0    0    phone_number_id_seq    SEQUENCE OWNED BY     U   ALTER SEQUENCE "public"."phone_number_id_seq" OWNED BY "public"."phone_number"."id";
          public          postgres    false    212            a           2604    24596 
   account id    DEFAULT     v   ALTER TABLE ONLY "public"."account" ALTER COLUMN "id" SET DEFAULT "nextval"('"public"."account_id_seq"'::"regclass");
 ?   ALTER TABLE "public"."account" ALTER COLUMN "id" DROP DEFAULT;
       public          postgres    false    210    209            b           2604    24597    phone_number id    DEFAULT     ?   ALTER TABLE ONLY "public"."phone_number" ALTER COLUMN "id" SET DEFAULT "nextval"('"public"."phone_number_id_seq"'::"regclass");
 D   ALTER TABLE "public"."phone_number" ALTER COLUMN "id" DROP DEFAULT;
       public          postgres    false    212    211            ?          0    24588    account 
   TABLE DATA                 public          postgres    false    209   ?       ?          0    24592    phone_number 
   TABLE DATA                 public          postgres    false    211   ?       ?           0    0    account_id_seq    SEQUENCE SET     @   SELECT pg_catalog.setval('"public"."account_id_seq"', 8, true);
          public          postgres    false    210                        0    0    phone_number_id_seq    SEQUENCE SET     F   SELECT pg_catalog.setval('"public"."phone_number_id_seq"', 87, true);
          public          postgres    false    212            d           2606    24599    account account_pkey 
   CONSTRAINT     Z   ALTER TABLE ONLY "public"."account"
    ADD CONSTRAINT "account_pkey" PRIMARY KEY ("id");
 D   ALTER TABLE ONLY "public"."account" DROP CONSTRAINT "account_pkey";
       public            postgres    false    209            f           2606    24601    phone_number phone_number_pkey 
   CONSTRAINT     d   ALTER TABLE ONLY "public"."phone_number"
    ADD CONSTRAINT "phone_number_pkey" PRIMARY KEY ("id");
 N   ALTER TABLE ONLY "public"."phone_number" DROP CONSTRAINT "phone_number_pkey";
       public            postgres    false    211            g           2606    24602 )   phone_number phone_number_account_id_fkey    FK CONSTRAINT     ?   ALTER TABLE ONLY "public"."phone_number"
    ADD CONSTRAINT "phone_number_account_id_fkey" FOREIGN KEY ("account_id") REFERENCES "public"."account"("id");
 Y   ALTER TABLE ONLY "public"."phone_number" DROP CONSTRAINT "phone_number_account_id_fkey";
       public          postgres    false    3172    209    211            ?   ?   x??ѻ?0 ?ᝧh??	1P
h\$???
((?b
?H????[ߠn?_???(? Q??ť.?????[???d?;???Usc?
??֣? ????H??,UQ?՘?h?<??ċvI?-N)?:r2??4p???4Y<???t|N<d?oK???^?
0??nn"?ǟu!?????~z^Q?^b?o      ?   ;  x???MK[Aཿ?r7WAʜ????.,???݊рBLD?????$?=??U??!??a??;'㝝?????fg߻??e???????w???j??0_<??a?w?????????e?|?y?b????\???O???????????;??n??J?,*y8????.OO??G?f???;????%?(?C1?%h?Q???(i?@??d?*,P?`?(?Xh?y+?%%????L5n?M8A??)???B???)?''d?PY%fU+?]
?7J?!d`?B?S?]
GA?nL???h?S?+?eJ????R?cQl?+?H?YK?J?zk?ć?f %;J" ?8J???ש?ǽPPR??TeĄ?(?QK??0?"?@?l?á??????HqAI??????,j1s??H?mm&???? ײ??6we%תd??촵?i?۶6??#ŀ?V?i??????A?Q?v?lӶ????????b"!?nEɎb?Gi?
?b?()??Vs=?U
??X?8.e"??(??????t#?c,9em*`?Yt?A??4?J?T"?<)N??1??0PBӵ???$y???R?2?}LFI\Ʋ %?v????m?(??X&?$O?	)PR?e?$??e??^%,,????c?f??$⒍?u{?%.c%%???H?A?:.c)?}lJ\?'ɛ???l?Y????2}
?S6??n?\??_????P_4Vi???FѨ?^l?l??l?/)YT???]?7??$??~|?D7?qH6؆o?W???V???eQ,Z?Y?)G˶^|>????_Gp:~???$7?H%???m????_h? ̔U7??p?? ??     