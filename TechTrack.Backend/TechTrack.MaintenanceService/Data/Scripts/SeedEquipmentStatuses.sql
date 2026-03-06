INSERT INTO equipment_status (id, name, description) VALUES
    -- Green - Operational (1-10)
    (1, 'Green', 'Оборудование полностью функционально'),
    
    -- Yellow - Warning (11-20)
    (2, 'Yellow', 'Имеет незначительные проблемы'),
    
    -- Red - Problem (21-30)
    (3, 'Red', 'Критическая неисправность'),
    
    -- Gray - Inactive (31-40)
    (4, 'Gray', 'Неактивно');