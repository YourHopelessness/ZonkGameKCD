SELECT DISTINCT ar.id AS api_resource_id,
    ar.api_name,
    ar.route,
	ar.http_method,
    p.id AS permission_id,
    p.name AS permission_name,
	r.id as role_id,
	r.name as role_name,
        CASE
            WHEN arp.* IS NOT NULL THEN true
            ELSE false
        END AS is_checked
   FROM api_resource ar
     CROSS JOIN permission p
     LEFT JOIN api_resource_permission arp ON p.id = arp.permission_id
	 left join role_permission as rp on rp.permission_id = p.id
	 left join role as r on r.id = rp.role_id;